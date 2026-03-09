'use client';

import { useState } from 'react';
import { Icons } from '../Icons';

interface Breakpoint {
  id: string;
  lineNumber: number;
  enabled: boolean;
  condition?: string;
}

interface Variable {
  name: string;
  value: string;
  type: string;
}

interface DebuggerPanelProps {
  isOpen: boolean;
  onClose: () => void;
  onBreakpointToggle?: (lineNumber: number) => void;
  breakpoints?: Breakpoint[];
}

export function DebuggerPanel({
  isOpen,
  onClose,
  onBreakpointToggle,
  breakpoints = [],
}: DebuggerPanelProps) {
  const [isDebugging, setIsDebugging] = useState(false);
  const [isPaused, setIsPaused] = useState(false);
  const [currentLine, setCurrentLine] = useState<number | null>(null);
  const [variables, setVariables] = useState<Variable[]>([
    { name: 'count', value: '0', type: 'int' },
    { name: 'message', value: '"Hello World"', type: 'string' },
    { name: 'isActive', value: 'true', type: 'bool' },
  ]);
  const [callStack, setCallStack] = useState<string[]>([
    'Main() at Program.cs:15',
    'ProcessData() at DataProcessor.cs:42',
    'ValidateInput() at Validator.cs:28',
  ]);
  const [watchExpressions, setWatchExpressions] = useState<string[]>([
    'count > 0',
    'message.Length',
  ]);
  const [newWatch, setNewWatch] = useState('');

  const handleStartDebugging = () => {
    setIsDebugging(true);
    setIsPaused(false);
    setCurrentLine(null);
  };

  const handleStopDebugging = () => {
    setIsDebugging(false);
    setIsPaused(false);
    setCurrentLine(null);
  };

  const handlePause = () => {
    setIsPaused(true);
    setCurrentLine(15); // Simulate pausing at line 15
  };

  const handleContinue = () => {
    setIsPaused(false);
    setCurrentLine(null);
  };

  const handleStepOver = () => {
    if (currentLine !== null) {
      setCurrentLine(currentLine + 1);
    }
  };

  const handleStepInto = () => {
    if (currentLine !== null) {
      setCurrentLine(currentLine + 1);
      // Simulate stepping into a method
      setCallStack((prev) => [...prev, `NewMethod() at Helper.cs:${currentLine + 1}`]);
    }
  };

  const handleStepOut = () => {
    if (callStack.length > 1) {
      setCallStack((prev) => prev.slice(0, -1));
      if (currentLine !== null) {
        setCurrentLine(currentLine + 5);
      }
    }
  };

  const handleAddWatch = () => {
    if (newWatch.trim()) {
      setWatchExpressions((prev) => [...prev, newWatch.trim()]);
      setNewWatch('');
    }
  };

  const handleRemoveWatch = (index: number) => {
    setWatchExpressions((prev) => prev.filter((_, i) => i !== index));
  };

  if (!isOpen) return null;

  return (
    <div className="bg-gray-900 border-t border-gray-700 h-96 flex flex-col">
      {/* Debugger Header */}
      <div className="flex items-center justify-between px-4 py-2 bg-gray-800 border-b border-gray-700">
        <div className="flex items-center gap-2">
          <Icons.Bug className="w-4 h-4 text-red-400" />
          <span className="text-sm font-medium text-gray-300">Debugger</span>
          {isDebugging && (
            <span className="px-2 py-0.5 text-xs bg-green-600 text-white rounded">
              {isPaused ? 'Paused' : 'Running'}
            </span>
          )}
        </div>
        <button
          onClick={onClose}
          className="p-1 hover:bg-gray-700 rounded"
          title="Close debugger"
        >
          <Icons.X className="w-4 h-4 text-gray-400" />
        </button>
      </div>

      {/* Debug Controls */}
      <div className="flex items-center gap-2 px-4 py-2 bg-gray-800 border-b border-gray-700">
        {!isDebugging ? (
          <button
            onClick={handleStartDebugging}
            className="flex items-center gap-1 px-3 py-1 bg-green-600 hover:bg-green-700 text-white rounded text-sm"
          >
            <Icons.Play className="w-4 h-4" />
            Start Debugging
          </button>
        ) : (
          <>
            <button
              onClick={handleStopDebugging}
              className="flex items-center gap-1 px-3 py-1 bg-red-600 hover:bg-red-700 text-white rounded text-sm"
            >
              <Icons.Stop className="w-4 h-4" />
              Stop
            </button>
            {!isPaused ? (
              <button
                onClick={handlePause}
                className="flex items-center gap-1 px-3 py-1 bg-yellow-600 hover:bg-yellow-700 text-white rounded text-sm"
              >
                <Icons.Pause className="w-4 h-4" />
                Pause
              </button>
            ) : (
              <>
                <button
                  onClick={handleContinue}
                  className="flex items-center gap-1 px-3 py-1 bg-blue-600 hover:bg-blue-700 text-white rounded text-sm"
                >
                  <Icons.Play className="w-4 h-4" />
                  Continue
                </button>
                <div className="w-px h-6 bg-gray-600" />
                <button
                  onClick={handleStepOver}
                  className="p-1 hover:bg-gray-700 rounded"
                  title="Step Over (F10)"
                >
                  <Icons.ArrowRight className="w-4 h-4 text-gray-300" />
                </button>
                <button
                  onClick={handleStepInto}
                  className="p-1 hover:bg-gray-700 rounded"
                  title="Step Into (F11)"
                >
                  <Icons.ArrowDown className="w-4 h-4 text-gray-300" />
                </button>
                <button
                  onClick={handleStepOut}
                  className="p-1 hover:bg-gray-700 rounded"
                  title="Step Out (Shift+F11)"
                >
                  <Icons.ArrowUp className="w-4 h-4 text-gray-300" />
                </button>
              </>
            )}
          </>
        )}
      </div>

      {/* Debugger Content */}
      <div className="flex-1 overflow-y-auto">
        <div className="grid grid-cols-2 gap-4 p-4">
          {/* Variables Panel */}
          <div className="space-y-2">
            <h3 className="text-sm font-semibold text-gray-300 mb-2">Variables</h3>
            <div className="space-y-1">
              {variables.map((variable, index) => (
                <div
                  key={index}
                  className="flex items-center justify-between p-2 bg-gray-800 rounded text-sm"
                >
                  <div className="flex items-center gap-2">
                    <span className="text-blue-400">{variable.name}</span>
                    <span className="text-gray-500">{variable.type}</span>
                  </div>
                  <span className="text-green-400">{variable.value}</span>
                </div>
              ))}
            </div>
          </div>

          {/* Call Stack Panel */}
          <div className="space-y-2">
            <h3 className="text-sm font-semibold text-gray-300 mb-2">Call Stack</h3>
            <div className="space-y-1">
              {callStack.map((frame, index) => (
                <div
                  key={index}
                  className={`p-2 rounded text-sm ${
                    index === callStack.length - 1
                      ? 'bg-blue-900 text-blue-200'
                      : 'bg-gray-800 text-gray-400'
                  }`}
                >
                  {frame}
                </div>
              ))}
            </div>
          </div>

          {/* Breakpoints Panel */}
          <div className="space-y-2">
            <h3 className="text-sm font-semibold text-gray-300 mb-2">Breakpoints</h3>
            <div className="space-y-1">
              {breakpoints.length === 0 ? (
                <p className="text-sm text-gray-500">No breakpoints set</p>
              ) : (
                breakpoints.map((bp) => (
                  <div
                    key={bp.id}
                    className="flex items-center justify-between p-2 bg-gray-800 rounded text-sm"
                  >
                    <div className="flex items-center gap-2">
                      <div
                        className={`w-3 h-3 rounded-full ${
                          bp.enabled ? 'bg-red-500' : 'bg-gray-600'
                        }`}
                      />
                      <span className="text-gray-300">Line {bp.lineNumber}</span>
                    </div>
                    <button
                      onClick={() => onBreakpointToggle?.(bp.lineNumber)}
                      className="text-xs text-gray-500 hover:text-gray-300"
                    >
                      Remove
                    </button>
                  </div>
                ))
              )}
            </div>
          </div>

          {/* Watch Expressions Panel */}
          <div className="space-y-2">
            <h3 className="text-sm font-semibold text-gray-300 mb-2">Watch</h3>
            <div className="space-y-1">
              {watchExpressions.map((expr, index) => (
                <div
                  key={index}
                  className="flex items-center justify-between p-2 bg-gray-800 rounded text-sm"
                >
                  <span className="text-gray-300">{expr}</span>
                  <button
                    onClick={() => handleRemoveWatch(index)}
                    className="text-xs text-gray-500 hover:text-gray-300"
                  >
                    <Icons.X className="w-3 h-3" />
                  </button>
                </div>
              ))}
              <div className="flex gap-2">
                <input
                  type="text"
                  value={newWatch}
                  onChange={(e) => setNewWatch(e.target.value)}
                  onKeyDown={(e) => e.key === 'Enter' && handleAddWatch()}
                  placeholder="Add watch expression..."
                  className="flex-1 px-2 py-1 bg-gray-800 border border-gray-700 rounded text-sm text-gray-300 outline-none focus:border-blue-500"
                />
                <button
                  onClick={handleAddWatch}
                  className="px-3 py-1 bg-blue-600 hover:bg-blue-700 text-white rounded text-sm"
                >
                  Add
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>

      {/* Current Line Indicator */}
      {isPaused && currentLine !== null && (
        <div className="px-4 py-2 bg-yellow-900 border-t border-yellow-700 text-sm text-yellow-200">
          <span className="font-semibold">Paused at line {currentLine}</span>
        </div>
      )}
    </div>
  );
}
