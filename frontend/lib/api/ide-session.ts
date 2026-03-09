import apiClient from '../api-client';

export interface IdeFile {
  path: string;
  content: string;
  language: string;
}

export interface CursorPosition {
  line: number;
  column: number;
}

export interface IdeSessionState {
  openFiles: IdeFile[];
  activeFile: string | null;
  cursorPositions: Record<string, CursorPosition>;
}

export interface IdeSessionResponse {
  sessionId: string;
  userId: string;
  sessionState: IdeSessionState;
  lastSavedAt: string;
}

/**
 * Save IDE session state to backend
 */
export async function saveIdeSession(userId: string, sessionState: IdeSessionState): Promise<void> {
  await apiClient.post('/api/ide/session', {
    userId,
    sessionState,
  });
}

/**
 * Load IDE session state from backend
 */
export async function loadIdeSession(userId: string): Promise<IdeSessionResponse | null> {
  try {
    const response = await apiClient.get<IdeSessionResponse>(`/api/ide/session/${userId}`);
    return response.data;
  } catch (error: any) {
    if (error.response?.status === 404) {
      return null; // No session found
    }
    throw error;
  }
}

/**
 * Delete IDE session
 */
export async function deleteIdeSession(userId: string): Promise<void> {
  await apiClient.delete(`/api/ide/session/${userId}`);
}
