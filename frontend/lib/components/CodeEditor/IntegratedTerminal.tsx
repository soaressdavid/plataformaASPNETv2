'use client';

import { useState, useRef, useEffect } from 'react';
import { Icons } from '../Icons';

interface TerminalProps {
  isOpen: boolean;
  onClose: () => void;
  height?: number;
}

interface TerminalLine {
  id: string;
  type: 'command' | 'output' | 'error';
  content: string;
  timestamp: Date;
}

export function IntegratedTerminal({ isOpen, onClose, height = 300 }: TerminalProps) {
  const [lines, setLines] = useState<TerminalLine[]>([
    {
      id: '1',
      type: 'output',
      content: 'Welcome to ASP.NET Learning Platform Terminal',
      timestamp: new Date(),
    },
    {
      id: '2',
      type: 'output',
      content: 'Type "help" for available commands',
      timestamp: new Date(),
    },
  ]);
  const [currentCommand, setCurrentCommand] = useState('');
  const [commandHistory, setCommandHistory] = useState<string[]>([]);
  const [historyIndex, setHistoryIndex] = useState(-1);
  const terminalRef = useRef<HTMLDivElement>(null);
  const inputRef = useRef<HTMLInputElement>(null);

  // Auto-scroll to bottom when new lines are added
  useEffect(() => {
    if (terminalRef.current) {
      terminalRef.current.scrollTop = terminalRef.current.scrollHeight;
    }
  }, [lines]);

  // Focus input when terminal opens
  useEffect(() => {
    if (isOpen && inputRef.current) {
      inputRef.current.focus();
    }
  }, [isOpen]);

  const addLine = (type: TerminalLine['type'], content: string) => {
    setLines((prev) => [
      ...prev,
      {
        id: Date.now().toString(),
        type,
        content,
        timestamp: new Date(),
      },
    ]);
  };

  const executeCommand = async (command: string) => {
    const trimmedCommand = command.trim();
    if (!trimmedCommand) return;

    // Add command to history
    setCommandHistory((prev) => [...prev, trimmedCommand]);
    setHistoryIndex(-1);

    // Add command to terminal
    addLine('command', `$ ${trimmedCommand}`);

    // Parse and execute command
    const [cmd, ...args] = trimmedCommand.split(' ');

    switch (cmd.toLowerCase()) {
      case 'help':
        addLine('output', 'Available commands:');
        addLine('output', '  help          - Show this help message');
        addLine('output', '  clear         - Clear terminal');
        addLine('output', '  echo <text>   - Print text to terminal');
        addLine('output', '  date          - Show current date and time');
        addLine('output', '  pwd           - Print working directory');
        addLine('output', '  ls            - List files (simulated)');
        addLine('output', '  dotnet --version - Show .NET version');
        addLine('output', '  git status    - Show git status (simulated)');
        break;

      case 'clear':
        setLines([]);
        break;

      case 'echo':
        addLine('output', args.join(' '));
        break;

      case 'date':
        addLine('output', new Date().toString());
        break;

      case 'pwd':
        addLine('output', '/workspace/project');
        break;

      case 'ls':
        addLine('output', 'Program.cs');
        addLine('output', 'Startup.cs');
        addLine('output', 'appsettings.json');
        addLine('output', 'Controllers/');
        addLine('output', 'Models/');
        break;

      case 'dotnet':
        if (args[0] === '--version') {
          addLine('output', '8.0.100');
        } else if (args[0] === 'build') {
          addLine('output', 'Building project...');
          await new Promise((resolve) => setTimeout(resolve, 1000));
          addLine('output', 'Build succeeded.');
        } else if (args[0] === 'run') {
          addLine('output', 'Running application...');
          addLine('output', 'Application started. Press Ctrl+C to shut down.');
        } else {
          addLine('output', 'Usage: dotnet [options]');
          addLine('output', '  --version    Show .NET version');
          addLine('output', '  build        Build the project');
          addLine('output', '  run          Run the application');
        }
        break;

      case 'git':
        if (args[0] === 'status') {
          addLine('output', 'On branch main');
          addLine('output', 'Your branch is up to date with \'origin/main\'.');
          addLine('output', '');
          addLine('output', 'nothing to commit, working tree clean');
        } else {
          addLine('output', 'Usage: git [command]');
          addLine('output', '  status       Show working tree status');
        }
        break;

      case 'exit':
        addLine('output', 'Closing terminal...');
        setTimeout(onClose, 500);
        break;

      default:
        addLine('error', `Command not found: ${cmd}`);
        addLine('output', 'Type "help" for available commands');
        break;
    }
  };

  const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === 'Enter') {
      executeCommand(currentCommand);
      setCurrentCommand('');
    } else if (e.key === 'ArrowUp') {
      e.preventDefault();
      if (commandHistory.length > 0) {
        const newIndex = historyIndex === -1 ? commandHistory.length - 1 : Math.max(0, historyIndex - 1);
        setHistoryIndex(newIndex);
        setCurrentCommand(commandHistory[newIndex]);
      }
    } else if (e.key === 'ArrowDown') {
      e.preventDefault();
      if (historyIndex !== -1) {
        const newIndex = historyIndex + 1;
        if (newIndex >= commandHistory.length) {
          setHistoryIndex(-1);
          setCurrentCommand('');
        } else {
          setHistoryIndex(newIndex);
          setCurrentCommand(commandHistory[newIndex]);
        }
      }
    } else if (e.key === 'Tab') {
      e.preventDefault();
      // Simple tab completion for common commands
      const commands = ['help', 'clear', 'echo', 'date', 'pwd', 'ls', 'dotnet', 'git', 'exit'];
      const matches = commands.filter((cmd) => cmd.startsWith(currentCommand));
      if (matches.length === 1) {
        setCurrentCommand(matches[0]);
      }
    }
  };

  if (!isOpen) return null;

  return (
    <div
      className="bg-gray-900 border-t border-gray-700 flex flex-col"
      style={{ height: `${height}px` }}
    >
      {/* Terminal Header */}
      <div className="flex items-center justify-between px-4 py-2 bg-gray-800 border-b border-gray-700">
        <div className="flex items-center gap-2">
          <Icons.Terminal className="w-4 h-4 text-green-400" />
          <span className="text-sm font-medium text-gray-300">Terminal</span>
        </div>
        <div className="flex items-center gap-2">
          <button
            onClick={() => setLines([])}
            className="p-1 hover:bg-gray-700 rounded"
            title="Clear terminal"
          >
            <Icons.X className="w-4 h-4 text-gray-400" />
          </button>
          <button
            onClick={onClose}
            className="p-1 hover:bg-gray-700 rounded"
            title="Close terminal"
          >
            <Icons.ChevronDown className="w-4 h-4 text-gray-400" />
          </button>
        </div>
      </div>

      {/* Terminal Content */}
      <div
        ref={terminalRef}
        className="flex-1 overflow-y-auto p-4 font-mono text-sm"
        onClick={() => inputRef.current?.focus()}
      >
        {lines.map((line) => (
          <div
            key={line.id}
            className={`mb-1 ${
              line.type === 'command'
                ? 'text-green-400'
                : line.type === 'error'
                ? 'text-red-400'
                : 'text-gray-300'
            }`}
          >
            {line.content}
          </div>
        ))}

        {/* Command Input */}
        <div className="flex items-center gap-2 text-green-400">
          <span>$</span>
          <input
            ref={inputRef}
            type="text"
            value={currentCommand}
            onChange={(e) => setCurrentCommand(e.target.value)}
            onKeyDown={handleKeyDown}
            className="flex-1 bg-transparent outline-none text-gray-300"
            placeholder="Type a command..."
            autoComplete="off"
            spellCheck={false}
          />
        </div>
      </div>

      {/* Terminal Footer */}
      <div className="px-4 py-1 bg-gray-800 border-t border-gray-700 text-xs text-gray-500">
        <span>Press Tab for autocomplete • ↑↓ for history • Type "help" for commands</span>
      </div>
    </div>
  );
}
