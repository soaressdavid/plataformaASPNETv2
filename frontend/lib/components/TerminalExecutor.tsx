'use client';

import React, { useState } from 'react';
import { Icons } from './Icons';

export function TerminalExecutor() {
  const [command, setCommand] = useState('');
  const [history, setHistory] = useState<Array<{ command: string; output: string }>>([
    { command: 'dotnet --version', output: '8.0.100' },
    { command: 'pwd', output: '/home/user/project' }
  ]);

  const commonCommands = [
    { label: 'dotnet build', cmd: 'dotnet build' },
    { label: 'dotnet run', cmd: 'dotnet run' },
    { label: 'dotnet publish', cmd: 'dotnet publish -c Release' },
    { label: 'git status', cmd: 'git status' },
    { label: 'docker ps', cmd: 'docker ps' },
  ];

  const handleExecute = () => {
    if (!command.trim()) return;

    // Simular execução de comandos
    let output = '';
    const cmd = command.toLowerCase().trim();

    if (cmd.includes('dotnet build')) {
      output = 'Build succeeded.\n    0 Warning(s)\n    0 Error(s)\n\nTime Elapsed 00:00:02.45';
    } else if (cmd.includes('dotnet run')) {
      output = 'info: Microsoft.Hosting.Lifetime[14]\n      Now listening on: http://localhost:5000\ninfo: Microsoft.Hosting.Lifetime[0]\n      Application started.';
    } else if (cmd.includes('dotnet publish')) {
      output = 'Publishing to /app/publish...\nPublish succeeded.';
    } else if (cmd.includes('git status')) {
      output = 'On branch main\nYour branch is up to date with \'origin/main\'.\n\nnothing to commit, working tree clean';
    } else if (cmd.includes('docker ps')) {
      output = 'CONTAINER ID   IMAGE          STATUS         PORTS\nabc123def456   myapp:latest   Up 2 minutes   0.0.0.0:5000->80/tcp';
    } else if (cmd.includes('ls')) {
      output = 'Program.cs\nStartup.cs\nappsettings.json\nbin/\nobj/';
    } else if (cmd.includes('pwd')) {
      output = '/home/user/project';
    } else {
      output = `Command '${command}' executed successfully.`;
    }

    setHistory([...history, { command, output }]);
    setCommand('');
  };

  const handleKeyPress = (e: React.KeyboardEvent) => {
    if (e.key === 'Enter') {
      handleExecute();
    }
  };

  return (
    <div className="flex flex-col h-full bg-gray-900 text-green-400 font-mono">
      {/* Header */}
      <div className="flex-shrink-0 p-3 border-b border-gray-700 bg-gray-800">
        <div className="flex items-center justify-between">
          <span className="text-xs font-medium text-gray-300">Terminal</span>
          <div className="flex gap-2">
            {commonCommands.map((cmd, index) => (
              <button
                key={index}
                onClick={() => setCommand(cmd.cmd)}
                className="px-2 py-1 text-xs bg-gray-700 text-gray-300 rounded hover:bg-gray-600"
                title={cmd.cmd}
              >
                {cmd.label}
              </button>
            ))}
          </div>
        </div>
      </div>

      {/* Terminal Output */}
      <div className="flex-1 overflow-auto p-4 space-y-2 text-sm">
        {history.map((item, index) => (
          <div key={index}>
            <div className="flex items-center gap-2">
              <span className="text-blue-400">$</span>
              <span className="text-white">{item.command}</span>
            </div>
            <div className="ml-4 text-green-400 whitespace-pre-wrap">{item.output}</div>
          </div>
        ))}
      </div>

      {/* Input */}
      <div className="flex-shrink-0 p-3 border-t border-gray-700 bg-gray-800">
        <div className="flex items-center gap-2">
          <span className="text-blue-400">$</span>
          <input
            type="text"
            value={command}
            onChange={(e) => setCommand(e.target.value)}
            onKeyPress={handleKeyPress}
            className="flex-1 bg-transparent border-none outline-none text-white text-sm"
            placeholder="Digite um comando..."
            autoFocus
          />
          <button
            onClick={handleExecute}
            className="px-3 py-1 text-xs bg-green-600 text-white rounded hover:bg-green-700"
          >
            Enter
          </button>
        </div>
      </div>

      {/* Help */}
      <div className="flex-shrink-0 p-2 bg-gray-800 border-t border-gray-700">
        <details className="text-xs text-gray-400">
          <summary className="cursor-pointer flex items-center gap-2">
            <Icons.LightBulb className="w-3 h-3" />
            Comandos Disponíveis
          </summary>
          <div className="mt-2 space-y-1 ml-4">
            <div>• dotnet build - Compilar projeto</div>
            <div>• dotnet run - Executar aplicação</div>
            <div>• dotnet publish - Publicar para produção</div>
            <div>• git status - Ver status do Git</div>
            <div>• docker ps - Listar containers</div>
            <div>• ls - Listar arquivos</div>
            <div>• pwd - Diretório atual</div>
          </div>
        </details>
      </div>
    </div>
  );
}
