'use client';

import { useState } from 'react';
import { Prism as SyntaxHighlighter } from 'react-syntax-highlighter';
import { vscDarkPlus } from 'react-syntax-highlighter/dist/cjs/styles/prism';
import { Icons } from './Icons';

export interface CodeExampleProps {
  title: string;
  code: string;
  language: string;
  explanation: string;
  isRunnable: boolean;
  onRun?: (code: string) => void;
}

/**
 * Component to render code examples with syntax highlighting.
 * Supports multiple languages, line numbers, copy functionality, and optional run button.
 * 
 * @param title - Title of the code example
 * @param code - Code content to display
 * @param language - Programming language for syntax highlighting
 * @param explanation - Explanation text displayed below the code
 * @param isRunnable - Whether the code can be executed
 * @param onRun - Optional callback function when run button is clicked
 */
export function CodeExample({
  title,
  code,
  language,
  explanation,
  isRunnable,
  onRun,
}: CodeExampleProps) {
  const [copied, setCopied] = useState(false);

  const handleCopy = async () => {
    try {
      await navigator.clipboard.writeText(code);
      setCopied(true);
      setTimeout(() => setCopied(false), 2000);
    } catch (err) {
      console.error('Failed to copy code:', err);
    }
  };

  const handleRun = () => {
    if (onRun) {
      onRun(code);
    }
  };

  if (!code) {
    return null;
  }

  return (
    <div className="code-example mb-8 rounded-xl overflow-hidden shadow-lg border-2 border-gray-200 hover:border-indigo-300 transition-all">
      <div className="code-example-header flex items-center justify-between px-5 py-4 bg-gradient-to-r from-gray-50 to-blue-50 border-b-2 border-gray-200">
        <h4 className="text-lg font-bold flex items-center gap-2.5 text-gray-800">
          <div className="w-8 h-8 bg-gradient-to-br from-indigo-500 to-purple-600 rounded-lg flex items-center justify-center">
            <Icons.Code className="w-5 h-5 text-white" />
          </div>
          {title}
        </h4>
        <div className="flex items-center gap-2">
          <button
            onClick={handleCopy}
            className="flex items-center gap-2 px-4 py-2 text-sm font-semibold bg-white border-2 border-gray-300 rounded-lg hover:bg-gray-50 hover:border-indigo-400 transition-all shadow-sm"
            style={{ color: '#374151' }}
            aria-label="Copy code to clipboard"
          >
            {copied ? (
              <>
                <Icons.Check className="w-4 h-4 text-green-500" />
                <span className="text-green-600">Copiado!</span>
              </>
            ) : (
              <>
                <Icons.ClipboardList className="w-4 h-4" />
                <span>Copiar</span>
              </>
            )}
          </button>
          {isRunnable && onRun && (
            <button
              onClick={handleRun}
              className="flex items-center gap-2 px-4 py-2 text-sm font-semibold text-white bg-gradient-to-r from-green-500 to-emerald-600 hover:from-green-600 hover:to-emerald-700 rounded-lg transition-all shadow-md hover:shadow-lg"
              aria-label="Run code"
            >
              <Icons.Play className="w-4 h-4" />
              <span>Executar</span>
            </button>
          )}
        </div>
      </div>
      
      <div className="code-example-body">
        <SyntaxHighlighter
          language={language}
          style={vscDarkPlus}
          showLineNumbers
          customStyle={{
            margin: 0,
            borderRadius: 0,
            fontSize: '0.875rem',
          }}
          codeTagProps={{
            style: {
              fontFamily: 'ui-monospace, SFMono-Regular, Menlo, Monaco, Consolas, monospace',
            },
          }}
        >
          {code}
        </SyntaxHighlighter>
      </div>
      
      {explanation && (
        <div className="code-explanation px-5 py-4 bg-gradient-to-r from-blue-50 to-indigo-50 border-t-2 border-gray-200">
          <div className="flex items-start gap-3">
            <div className="w-6 h-6 bg-blue-500 rounded-full flex items-center justify-center shrink-0 mt-0.5">
              <svg className="w-4 h-4 text-white" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
              </svg>
            </div>
            <p className="text-base leading-relaxed text-gray-700 flex-1">
              {explanation}
            </p>
          </div>
        </div>
      )}
    </div>
  );
}
