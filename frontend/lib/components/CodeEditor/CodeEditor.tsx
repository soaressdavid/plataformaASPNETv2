'use client';

import React, { useState, useRef } from 'react';
import Editor from '@monaco-editor/react';
import type { editor } from 'monaco-editor';
import { CodeExecutionLoader } from '../LoadingSkeletons';
import { ExecutionStatusBadge } from '../ExecutionStatusBadge';
import { ExecutionStatus } from '../../types';
import { Icons } from '../Icons';

export interface CodeFile {
  name: string;
  content: string;
  language: string;
}

export interface CodeEditorProps {
  files: CodeFile[];
  activeFileIndex: number;
  onFileChange: (index: number, content: string) => void;
  onActiveFileChange: (index: number) => void;
  onAddFile?: (file: CodeFile) => void;
  onRemoveFile?: (index: number) => void;
  onRun: () => void;
  output: string;
  isRunning: boolean;
  executionStatus?: ExecutionStatus;
  executionTime?: number;
  onGetFeedback?: () => void;
  showAIFeedback?: boolean;
  aiFeedbackPanel?: React.ReactNode;
}

export const CodeEditor: React.FC<CodeEditorProps> = ({
  files,
  activeFileIndex,
  onFileChange,
  onActiveFileChange,
  onAddFile,
  onRemoveFile,
  onRun,
  output,
  isRunning,
  executionStatus,
  executionTime,
  onGetFeedback,
  showAIFeedback = false,
  aiFeedbackPanel,
}) => {
  const editorRef = useRef<editor.IStandaloneCodeEditor | null>(null);
  const [showOutput, setShowOutput] = useState(false);
  const [showAddFileDialog, setShowAddFileDialog] = useState(false);
  const [newFileName, setNewFileName] = useState('');

  // Automatically show output panel when code execution starts
  React.useEffect(() => {
    if (isRunning) {
      setShowOutput(true);
    }
  }, [isRunning]);

  const handleEditorDidMount = (editor: editor.IStandaloneCodeEditor) => {
    editorRef.current = editor;

    // Force focus on the editor to ensure keyboard events are captured
    editor.focus();

    // Configure IntelliSense for .NET APIs
    const monaco = (window as any).monaco;
    if (monaco) {
      // Configure C# language features
      monaco.languages.typescript.typescriptDefaults.setCompilerOptions({
        target: monaco.languages.typescript.ScriptTarget.ES2020,
        allowNonTsExtensions: true,
      });

      // Add .NET API suggestions
      monaco.languages.registerCompletionItemProvider('csharp', {
        provideCompletionItems: () => {
          const suggestions = [
            {
              label: 'Console.WriteLine',
              kind: monaco.languages.CompletionItemKind.Method,
              insertText: 'Console.WriteLine($1);',
              insertTextRules: monaco.languages.CompletionItemInsertTextRule.InsertAsSnippet,
              documentation: 'Writes a line to the console',
            },
            {
              label: 'Console.ReadLine',
              kind: monaco.languages.CompletionItemKind.Method,
              insertText: 'Console.ReadLine()',
              documentation: 'Reads a line from the console',
            },
            {
              label: 'string',
              kind: monaco.languages.CompletionItemKind.Keyword,
              insertText: 'string',
              documentation: 'System.String type',
            },
            {
              label: 'int',
              kind: monaco.languages.CompletionItemKind.Keyword,
              insertText: 'int',
              documentation: 'System.Int32 type',
            },
            {
              label: 'var',
              kind: monaco.languages.CompletionItemKind.Keyword,
              insertText: 'var',
              documentation: 'Implicitly typed variable',
            },
            {
              label: 'async',
              kind: monaco.languages.CompletionItemKind.Keyword,
              insertText: 'async',
              documentation: 'Async method modifier',
            },
            {
              label: 'await',
              kind: monaco.languages.CompletionItemKind.Keyword,
              insertText: 'await',
              documentation: 'Await async operation',
            },
            {
              label: 'Task',
              kind: monaco.languages.CompletionItemKind.Class,
              insertText: 'Task',
              documentation: 'System.Threading.Tasks.Task',
            },
            {
              label: 'Task<T>',
              kind: monaco.languages.CompletionItemKind.Class,
              insertText: 'Task<$1>',
              insertTextRules: monaco.languages.CompletionItemInsertTextRule.InsertAsSnippet,
              documentation: 'System.Threading.Tasks.Task<T>',
            },
            {
              label: 'List',
              kind: monaco.languages.CompletionItemKind.Class,
              insertText: 'List<$1>',
              insertTextRules: monaco.languages.CompletionItemInsertTextRule.InsertAsSnippet,
              documentation: 'System.Collections.Generic.List<T>',
            },
            {
              label: 'Dictionary',
              kind: monaco.languages.CompletionItemKind.Class,
              insertText: 'Dictionary<$1, $2>',
              insertTextRules: monaco.languages.CompletionItemInsertTextRule.InsertAsSnippet,
              documentation: 'System.Collections.Generic.Dictionary<TKey, TValue>',
            },
            {
              label: 'public class',
              kind: monaco.languages.CompletionItemKind.Snippet,
              insertText: 'public class ${1:ClassName}\n{\n\t$0\n}',
              insertTextRules: monaco.languages.CompletionItemInsertTextRule.InsertAsSnippet,
              documentation: 'Create a public class',
            },
            {
              label: 'public method',
              kind: monaco.languages.CompletionItemKind.Snippet,
              insertText: 'public ${1:void} ${2:MethodName}(${3})\n{\n\t$0\n}',
              insertTextRules: monaco.languages.CompletionItemInsertTextRule.InsertAsSnippet,
              documentation: 'Create a public method',
            },
          ];
          return { suggestions };
        },
      });
    }
  };

  const handleEditorChange = (value: string | undefined) => {
    if (value !== undefined) {
      onFileChange(activeFileIndex, value);
    }
  };

  const handleAddFile = () => {
    if (newFileName && onAddFile) {
      const language = newFileName.endsWith('.cs') ? 'csharp' : 'plaintext';
      onAddFile({
        name: newFileName,
        content: '',
        language,
      });
      setNewFileName('');
      setShowAddFileDialog(false);
    }
  };

  const handleRemoveFile = (index: number) => {
    if (onRemoveFile && files.length > 1) {
      onRemoveFile(index);
    }
  };

  const activeFile = files[activeFileIndex];

  return (
    <div className="flex h-full bg-gray-900">
      {/* Main Editor Area */}
      <div className="flex flex-col flex-1 min-w-0">
        {/* File Tabs */}
        <div className="flex items-center bg-gray-800 border-b border-gray-700">
          <div className="flex overflow-x-auto">
            {files.map((file, index) => (
              <div
                key={index}
                className={`flex items-center border-r border-gray-700 ${
                  index === activeFileIndex ? 'bg-gray-900' : 'bg-gray-800'
                }`}
              >
                <button
                  onClick={() => onActiveFileChange(index)}
                  className={`px-4 py-2 text-sm font-medium whitespace-nowrap ${
                    index === activeFileIndex
                      ? 'text-white'
                      : 'text-gray-400 hover:text-white hover:bg-gray-700'
                  }`}
                >
                  {file.name}
                </button>
                {files.length > 1 && onRemoveFile && (
                  <button
                    onClick={() => handleRemoveFile(index)}
                    className="px-2 text-gray-400 hover:text-red-400"
                    title="Remove file"
                  >
                    ×
                  </button>
                )}
              </div>
            ))}
            {onAddFile && (
              <button
                onClick={() => setShowAddFileDialog(true)}
                className="px-4 py-2 text-sm font-medium text-gray-400 hover:text-white hover:bg-gray-700"
                title="Add new file"
              >
                +
              </button>
            )}
          </div>
          <div className="ml-auto flex items-center gap-2 px-4">
            {onGetFeedback && (
              <button
                onClick={onGetFeedback}
                className="px-4 py-1.5 rounded text-sm font-medium bg-purple-600 text-white hover:bg-purple-700 flex items-center gap-2"
              >
                <Icons.Sparkles className="w-4 h-4" />
                <span>Obter Feedback</span>
              </button>
            )}
            <button
              onClick={onRun}
              disabled={isRunning}
              className={`px-4 py-1.5 rounded text-sm font-medium ${
                isRunning
                  ? 'bg-gray-600 text-gray-400 cursor-not-allowed'
                  : 'bg-green-600 text-white hover:bg-green-700'
              }`}
            >
              {isRunning ? 'Executando...' : '▶ Executar'}
            </button>
          </div>
        </div>

      {/* Add File Dialog */}
      {showAddFileDialog && (
        <div className="bg-gray-800 border-b border-gray-700 px-4 py-3 flex items-center gap-2">
          <input
            type="text"
            value={newFileName}
            onChange={(e) => setNewFileName(e.target.value)}
            placeholder="FileName.cs"
            className="px-3 py-1.5 bg-gray-900 text-white border border-gray-600 rounded text-sm focus:outline-none focus:border-blue-500"
            onKeyDown={(e) => {
              if (e.key === 'Enter') handleAddFile();
              if (e.key === 'Escape') setShowAddFileDialog(false);
            }}
            autoFocus
          />
          <button
            onClick={handleAddFile}
            className="px-3 py-1.5 bg-blue-600 text-white rounded text-sm hover:bg-blue-700"
          >
            Add
          </button>
          <button
            onClick={() => {
              setShowAddFileDialog(false);
              setNewFileName('');
            }}
            className="px-3 py-1.5 bg-gray-600 text-white rounded text-sm hover:bg-gray-700"
          >
            Cancel
          </button>
        </div>
      )}

      {/* Editor */}
      <div className="flex-1 overflow-hidden">
        <Editor
          height="100%"
          language={activeFile.language}
          value={activeFile.content}
          onChange={handleEditorChange}
          onMount={handleEditorDidMount}
          theme="vs-dark"
          options={{
            minimap: { enabled: true },
            fontSize: 14,
            lineNumbers: 'on',
            roundedSelection: false,
            scrollBeyondLastLine: false,
            automaticLayout: true,
            tabSize: 4,
            wordWrap: 'on',
            suggestOnTriggerCharacters: true,
            quickSuggestions: true,
            parameterHints: { enabled: true },
            formatOnPaste: true,
            formatOnType: true,
            acceptSuggestionOnCommitCharacter: true,
            acceptSuggestionOnEnter: 'on',
            accessibilitySupport: 'off',
            autoClosingBrackets: 'always',
            autoClosingQuotes: 'always',
            autoIndent: 'full',
            cursorBlinking: 'smooth',
            cursorSmoothCaretAnimation: 'on',
            dragAndDrop: true,
            folding: true,
            foldingStrategy: 'indentation',
            fontLigatures: true,
            links: true,
            mouseWheelZoom: true,
            multiCursorModifier: 'ctrlCmd',
            renderWhitespace: 'selection',
            selectOnLineNumbers: true,
            smoothScrolling: true,
            snippetSuggestions: 'top',
            tabCompletion: 'on',
            useTabStops: true,
            wordBasedSuggestions: 'matchingDocuments',
            bracketPairColorization: { enabled: true },
            contextmenu: true,
            copyWithSyntaxHighlighting: true,
            emptySelectionClipboard: false,
            find: {
              addExtraSpaceOnTop: false,
              autoFindInSelection: 'never',
              seedSearchStringFromSelection: 'always',
            },
          }}
        />
      </div>

      {/* Terminal Output Panel */}
      <div className="border-t border-gray-700">
        <button
          onClick={() => setShowOutput(!showOutput)}
          className="w-full px-4 py-2 text-left text-sm font-medium text-gray-300 bg-gray-800 hover:bg-gray-700 flex items-center justify-between"
        >
          <div className="flex items-center gap-3">
            <span>Terminal Output</span>
            {executionStatus && (
              <ExecutionStatusBadge status={executionStatus} executionTime={executionTime} />
            )}
          </div>
          <span>{showOutput ? '▼' : '▲'}</span>
        </button>
        {showOutput && (
          <div className="bg-black text-gray-100 p-4 font-mono text-sm h-48 overflow-y-auto">
            {isRunning ? (
              <CodeExecutionLoader />
            ) : output ? (
              <pre className="whitespace-pre-wrap">{output}</pre>
            ) : (
              <span className="text-gray-500">No output yet. Click Run to execute your code.</span>
            )}
          </div>
        )}
      </div>
    </div>

    {/* AI Feedback Panel */}
    {showAIFeedback && aiFeedbackPanel && (
      <div className="w-96 shrink-0">
        {aiFeedbackPanel}
      </div>
    )}
  </div>
  );
};
