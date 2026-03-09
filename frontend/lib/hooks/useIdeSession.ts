import { useState, useEffect, useCallback, useRef } from 'react';
import { saveIdeSession, loadIdeSession, IdeSessionState, IdeFile, CursorPosition } from '../api/ide-session';

interface UseIdeSessionOptions {
  userId: string;
  autoSaveInterval?: number; // in milliseconds, default 30000 (30 seconds)
  onSaveSuccess?: () => void;
  onSaveError?: (error: Error) => void;
  onLoadSuccess?: (state: IdeSessionState) => void;
  onLoadError?: (error: Error) => void;
}

interface UseIdeSessionReturn {
  sessionState: IdeSessionState;
  updateOpenFiles: (files: IdeFile[]) => void;
  updateActiveFile: (filePath: string | null) => void;
  updateCursorPosition: (filePath: string, position: CursorPosition) => void;
  saveNow: () => Promise<void>;
  isLoading: boolean;
  isSaving: boolean;
  lastSavedAt: Date | null;
}

/**
 * Hook for managing IDE session persistence with auto-save
 */
export function useIdeSession(options: UseIdeSessionOptions): UseIdeSessionReturn {
  const {
    userId,
    autoSaveInterval = 30000, // 30 seconds default
    onSaveSuccess,
    onSaveError,
    onLoadSuccess,
    onLoadError,
  } = options;

  const [sessionState, setSessionState] = useState<IdeSessionState>({
    openFiles: [],
    activeFile: null,
    cursorPositions: {},
  });

  const [isLoading, setIsLoading] = useState(true);
  const [isSaving, setIsSaving] = useState(false);
  const [lastSavedAt, setLastSavedAt] = useState<Date | null>(null);
  const [hasUnsavedChanges, setHasUnsavedChanges] = useState(false);

  const autoSaveTimerRef = useRef<NodeJS.Timeout | null>(null);
  const sessionStateRef = useRef(sessionState);

  // Keep ref in sync with state
  useEffect(() => {
    sessionStateRef.current = sessionState;
  }, [sessionState]);

  // Load session on mount
  useEffect(() => {
    const loadSession = async () => {
      try {
        setIsLoading(true);
        const session = await loadIdeSession(userId);
        
        if (session) {
          setSessionState(session.sessionState);
          setLastSavedAt(new Date(session.lastSavedAt));
          onLoadSuccess?.(session.sessionState);
        }
      } catch (error) {
        console.error('Failed to load IDE session:', error);
        onLoadError?.(error as Error);
      } finally {
        setIsLoading(false);
      }
    };

    loadSession();
  }, [userId, onLoadSuccess, onLoadError]);

  // Save session function
  const saveSession = useCallback(async () => {
    if (!hasUnsavedChanges) {
      return;
    }

    try {
      setIsSaving(true);
      await saveIdeSession(userId, sessionStateRef.current);
      setLastSavedAt(new Date());
      setHasUnsavedChanges(false);
      onSaveSuccess?.();
    } catch (error) {
      console.error('Failed to save IDE session:', error);
      onSaveError?.(error as Error);
    } finally {
      setIsSaving(false);
    }
  }, [userId, hasUnsavedChanges, onSaveSuccess, onSaveError]);

  // Auto-save timer
  useEffect(() => {
    if (autoSaveInterval > 0) {
      autoSaveTimerRef.current = setInterval(() => {
        saveSession();
      }, autoSaveInterval);

      return () => {
        if (autoSaveTimerRef.current) {
          clearInterval(autoSaveTimerRef.current);
        }
      };
    }
  }, [autoSaveInterval, saveSession]);

  // Save on unmount if there are unsaved changes
  useEffect(() => {
    return () => {
      if (hasUnsavedChanges) {
        // Use synchronous save on unmount
        saveIdeSession(userId, sessionStateRef.current).catch(console.error);
      }
    };
  }, [userId, hasUnsavedChanges]);

  // Update functions
  const updateOpenFiles = useCallback((files: IdeFile[]) => {
    setSessionState(prev => ({ ...prev, openFiles: files }));
    setHasUnsavedChanges(true);
  }, []);

  const updateActiveFile = useCallback((filePath: string | null) => {
    setSessionState(prev => ({ ...prev, activeFile: filePath }));
    setHasUnsavedChanges(true);
  }, []);

  const updateCursorPosition = useCallback((filePath: string, position: CursorPosition) => {
    setSessionState(prev => ({
      ...prev,
      cursorPositions: {
        ...prev.cursorPositions,
        [filePath]: position,
      },
    }));
    setHasUnsavedChanges(true);
  }, []);

  const saveNow = useCallback(async () => {
    await saveSession();
  }, [saveSession]);

  return {
    sessionState,
    updateOpenFiles,
    updateActiveFile,
    updateCursorPosition,
    saveNow,
    isLoading,
    isSaving,
    lastSavedAt,
  };
}
