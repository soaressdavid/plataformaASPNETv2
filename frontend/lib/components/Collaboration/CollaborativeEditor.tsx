'use client';

import React, { useEffect, useState, useRef, useCallback } from 'react';
import Editor, { Monaco } from '@monaco-editor/react';
import { editor } from 'monaco-editor';
import { useCollaboration, Operation, CursorPosition } from '@/lib/hooks/useCollaboration';

/**
 * Collaborative code editor with real-time synchronization
 * Validates: Requirements 32.2, 32.3, 32.7
 */

interface CollaborativeEditorProps {
  sessionId: string;
  initialCode?: string;
  language?: string;
  onCodeChange?: (code: string) => void;
  onRunCode?: () => void;
}

// Color palette for user cursors
const CURSOR_COLORS = [
  '#FF6B6B', // Red
  '#4ECDC4', // Teal
  '#45B7D1', // Blue
  '#FFA07A', // Light Salmon
  '#98D8C8', // Mint
  '#F7DC6F', // Yellow
  '#BB8FCE', // Purple
  '#85C1E2', // Sky Blue
];

export function CollaborativeEditor({
  sessionId,
  initialCode = '',
  language = 'csharp',
  onCodeChange,
  onRunCode,
}: CollaborativeEditorProps) {
  const [code, setCode] = useState(initialCode);
  const [isApplyingRemoteChange, setIsApplyingRemoteChange] = useState(false);
  const [userColors, setUserColors] = useState<Map<string, string>>(new Map());
  const editorRef = useRef<editor.IStandaloneCodeEditor | null>(null);
  const monacoRef = useRef<Monaco | null>(null);
  const decorationsRef = useRef<string[]>([]);
  const versionRef = useRef(0);

  const {
    isConnected,
    cursorPositions,
    sendOperation,
    updateCursor,
    runCode,
    onReceiveOperation,
    onCursorMoved,
    onCodeExecutionStarted,
  } = useCollaboration(process.env.NEXT_PUBLIC_COLLABORATION_HUB_URL || 'http://localhost:5007/hubs/collaboration');

  // Assign colors to users
  const getUserColor = useCallback((userId: string) => {
    if (!userColors.has(userId)) {
      const colorIndex = userColors.size % CURSOR_COLORS.length;
      const newColors = new Map(userColors);
      newColors.set(userId, CURSOR_COLORS[colorIndex]);
      setUserColors(newColors);
      return CURSOR_COLORS[colorIndex];
    }
    return userColors.get(userId)!;
  }, [userColors]);

  // Handle editor mount
  const handleEditorDidMount = (editor: editor.IStandaloneCodeEditor, monaco: Monaco) => {
    editorRef.current = editor;
    monacoRef.current = monaco;

    // Listen for cursor position changes
    editor.onDidChangeCursorPosition((e) => {
      if (!isApplyingRemoteChange && isConnected) {
        const position = e.position;
        const selection = editor.getSelection();
        
        updateCursor(
          sessionId,
          position.lineNumber,
          position.column,
          selection?.startLineNumber,
          selection?.startColumn
        );
      }
    });

    // Listen for content changes
    editor.onDidChangeModelContent((e) => {
      if (isApplyingRemoteChange) return;

      const model = editor.getModel();
      if (!model) return;

      const newCode = model.getValue();
      setCode(newCode);
      onCodeChange?.(newCode);

      // Send operations to other participants
      e.changes.forEach((change) => {
        const operation: Operation = {
          type: change.text ? 'Insert' : 'Delete',
          position: model.getOffsetAt({
            lineNumber: change.range.startLineNumber,
            column: change.range.startColumn,
          }),
          text: change.text || undefined,
          length: change.rangeLength,
          userId: '', // Will be set by the server
          version: versionRef.current,
        };

        sendOperation(sessionId, operation);
      });
    });
  };

  // Apply remote operation
  const applyRemoteOperation = useCallback((operation: Operation) => {
    if (!editorRef.current || !monacoRef.current) return;

    setIsApplyingRemoteChange(true);

    const model = editorRef.current.getModel();
    if (!model) {
      setIsApplyingRemoteChange(false);
      return;
    }

    try {
      const position = model.getPositionAt(operation.position);

      if (operation.type === 'Insert' && operation.text) {
        const range = new monacoRef.current.Range(
          position.lineNumber,
          position.column,
          position.lineNumber,
          position.column
        );
        model.pushEditOperations(
          [],
          [{ range, text: operation.text }],
          () => null
        );
      } else if (operation.type === 'Delete') {
        const endPosition = model.getPositionAt(operation.position + operation.length);
        const range = new monacoRef.current.Range(
          position.lineNumber,
          position.column,
          endPosition.lineNumber,
          endPosition.column
        );
        model.pushEditOperations(
          [],
          [{ range, text: '' }],
          () => null
        );
      }

      versionRef.current = operation.version;
      const newCode = model.getValue();
      setCode(newCode);
      onCodeChange?.(newCode);
    } finally {
      setIsApplyingRemoteChange(false);
    }
  }, [onCodeChange]);

  // Render remote cursors
  const renderRemoteCursors = useCallback(() => {
    if (!editorRef.current || !monacoRef.current) return;

    const decorations: editor.IModelDeltaDecoration[] = [];

    cursorPositions.forEach((cursor, userId) => {
      const color = getUserColor(userId);

      // Cursor decoration
      const cursorDecoration: editor.IModelDeltaDecoration = {
        range: new monacoRef.current!.Range(cursor.line, cursor.column, cursor.line, cursor.column),
        options: {
          className: 'remote-cursor',
          beforeContentClassName: 'remote-cursor-line',
          glyphMarginClassName: 'remote-cursor-glyph',
          stickiness: monacoRef.current!.editor.TrackedRangeStickiness.NeverGrowsWhenTypingAtEdges,
          after: {
            content: ` ${userId.substring(0, 8)}`,
            inlineClassName: 'remote-cursor-label',
            inlineClassNameAffectsLetterSpacing: true,
          },
        },
      };

      decorations.push(cursorDecoration);

      // Selection decoration
      if (cursor.selectionStartLine && cursor.selectionStartColumn) {
        const selectionDecoration: editor.IModelDeltaDecoration = {
          range: new monacoRef.current!.Range(
            cursor.selectionStartLine,
            cursor.selectionStartColumn,
            cursor.line,
            cursor.column
          ),
          options: {
            className: 'remote-selection',
            stickiness: monacoRef.current!.editor.TrackedRangeStickiness.NeverGrowsWhenTypingAtEdges,
          },
        };
        decorations.push(selectionDecoration);
      }

      // Add CSS for this user's cursor color
      const styleId = `cursor-style-${userId}`;
      if (!document.getElementById(styleId)) {
        const style = document.createElement('style');
        style.id = styleId;
        style.innerHTML = `
          .remote-cursor-${userId} {
            border-left: 2px solid ${color};
          }
          .remote-cursor-label-${userId} {
            background-color: ${color};
            color: white;
            padding: 0 4px;
            border-radius: 2px;
            font-size: 10px;
          }
          .remote-selection-${userId} {
            background-color: ${color}33;
          }
        `;
        document.head.appendChild(style);
      }
    });

    decorationsRef.current = editorRef.current.deltaDecorations(decorationsRef.current, decorations);
  }, [cursorPositions, getUserColor]);

  // Setup event listeners
  useEffect(() => {
    const unsubscribeOperation = onReceiveOperation(applyRemoteOperation);
    const unsubscribeCursor = onCursorMoved(() => {
      renderRemoteCursors();
    });
    const unsubscribeExecution = onCodeExecutionStarted((data) => {
      console.log('Code execution started by user:', data.userId);
    });

    return () => {
      unsubscribeOperation();
      unsubscribeCursor();
      unsubscribeExecution();
    };
  }, [onReceiveOperation, onCursorMoved, onCodeExecutionStarted, applyRemoteOperation, renderRemoteCursors]);

  // Update cursor decorations when positions change
  useEffect(() => {
    renderRemoteCursors();
  }, [cursorPositions, renderRemoteCursors]);

  const handleRunCode = () => {
    runCode(sessionId);
    onRunCode?.();
  };

  return (
    <div className="collaborative-editor h-full flex flex-col">
      <div className="editor-toolbar flex items-center justify-between p-2 bg-gray-100 border-b">
        <div className="flex items-center gap-2">
          <div className={`w-2 h-2 rounded-full ${isConnected ? 'bg-green-500' : 'bg-red-500'}`} />
          <span className="text-sm text-gray-600">
            {isConnected ? 'Connected' : 'Disconnected'}
          </span>
        </div>
        <div className="flex items-center gap-2">
          <span className="text-sm text-gray-600">
            {cursorPositions.size} collaborator{cursorPositions.size !== 1 ? 's' : ''}
          </span>
          <button
            onClick={handleRunCode}
            disabled={!isConnected}
            className="px-4 py-1 bg-green-600 text-white rounded hover:bg-green-700 disabled:bg-gray-400 disabled:cursor-not-allowed"
          >
            Run Code
          </button>
        </div>
      </div>
      <div className="flex-1">
        <Editor
          height="100%"
          language={language}
          value={code}
          onMount={handleEditorDidMount}
          options={{
            minimap: { enabled: true },
            fontSize: 14,
            lineNumbers: 'on',
            automaticLayout: true,
            suggestOnTriggerCharacters: true,
            readOnly: !isConnected,
          }}
        />
      </div>
      <style jsx global>{`
        .remote-cursor {
          position: relative;
        }
        .remote-cursor-line {
          border-left: 2px solid;
          position: absolute;
          height: 100%;
        }
        .remote-cursor-label {
          position: absolute;
          top: -20px;
          left: 0;
          padding: 2px 4px;
          border-radius: 2px;
          font-size: 10px;
          white-space: nowrap;
          z-index: 1000;
        }
        .remote-selection {
          opacity: 0.3;
        }
      `}</style>
    </div>
  );
}
