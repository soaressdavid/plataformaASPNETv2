'use client';

import React, { useState } from 'react';
import { CodeEditor, CodeFile, AIFeedback } from '@/lib/components';
import { useCodeExecution } from '@/lib/hooks/useCodeExecution';
import { useAIFeedback } from '@/lib/hooks/useAIFeedback';
import { ExecutionStatus } from '@/lib/types';
import { Icons } from '@/lib/components/Icons';

const initialFiles: CodeFile[] = [
  {
    name: 'Program.cs',
    language: 'csharp',
    content: `using System;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, ASP.NET Core Learning Platform!");
            
            // Try writing your own code here
            int sum = 5 + 10;
            Console.WriteLine($"5 + 10 = {sum}");
        }
    }
}`,
  },
  {
    name: 'Helper.cs',
    language: 'csharp',
    content: `using System;

namespace HelloWorld
{
    public class Helper
    {
        public static string GetGreeting(string name)
        {
            return $"Hello, {name}!";
        }
    }
}`,
  },
];

export default function IDEPage() {
  const [files, setFiles] = useState<CodeFile[]>(initialFiles);
  const [activeFileIndex, setActiveFileIndex] = useState(0);
  const [showAIFeedback, setShowAIFeedback] = useState(false);

  // Use the code execution hook with WebSocket support
  const { execute, isRunning, output, error, status, executionTime } = useCodeExecution({
    useWebSocket: true,
  });

  // Use the AI feedback hook
  const { feedback, isLoading: isLoadingFeedback, getFeedback, clearFeedback } = useAIFeedback();

  const handleFileChange = (index: number, content: string) => {
    const newFiles = [...files];
    newFiles[index].content = content;
    setFiles(newFiles);
  };

  const handleAddFile = (file: CodeFile) => {
    setFiles([...files, file]);
    setActiveFileIndex(files.length);
  };

  const handleRemoveFile = (index: number) => {
    if (files.length > 1) {
      const newFiles = files.filter((_, i) => i !== index);
      setFiles(newFiles);
      if (activeFileIndex >= newFiles.length) {
        setActiveFileIndex(newFiles.length - 1);
      }
    }
  };

  const handleRun = async () => {
    // Prepare code for execution
    const codeFiles = files.map((file) => ({
      name: file.name,
      content: file.content,
    }));

    await execute({
      code: files[0].content, // Main file
      files: codeFiles,
      entryPoint: 'Program.cs',
    });
  };

  const handleGetFeedback = async () => {
    // Get the active file's code for review
    const activeFile = files[activeFileIndex];
    const allCode = files.map((f) => `// ${f.name}\n${f.content}`).join('\n\n');
    
    setShowAIFeedback(true);
    await getFeedback(allCode, `Reviewing ${activeFile.name} in an ASP.NET Core learning context`);
  };

  const handleCloseFeedback = () => {
    setShowAIFeedback(false);
    clearFeedback();
  };

  // Format output with status information
  const getFormattedOutput = () => {
    if (!status) {
      return 'Nenhuma saída ainda. Clique em Executar para executar seu código.';
    }

    let result = '';

    // Add status indicator
    switch (status.status) {
      case ExecutionStatus.Queued:
        result += 'Na Fila - Aguardando execução...\n\n';
        break;
      case ExecutionStatus.Running:
        result += 'Executando - Executando seu código...\n\n';
        break;
      case ExecutionStatus.Completed:
        result += 'Concluído\n\n';
        break;
      case ExecutionStatus.Failed:
        result += 'Falhou\n\n';
        break;
      case ExecutionStatus.Timeout:
        result += 'Tempo Esgotado - Execução excedeu o limite de 30 segundos\n\n';
        break;
      case ExecutionStatus.MemoryExceeded:
        result += 'Memória Excedida - Execução excedeu o limite de 512MB de memória\n\n';
        break;
    }

    // Add output
    if (output) {
      result += 'Saída:\n';
      result += output + '\n';
    }

    // Add error
    if (error) {
      result += '\nErro:\n';
      result += error + '\n';
    }

    // Add execution time for completed executions
    if (
      status.status === ExecutionStatus.Completed ||
      status.status === ExecutionStatus.Failed
    ) {
      result += `\nTempo de execução: ${executionTime}ms`;
    }

    return result;
  };

  return (
    <div className="h-screen flex flex-col bg-gray-900">
      {/* Header */}
      <header className="bg-gray-800 border-b border-gray-700 px-6 py-4">
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-2xl font-bold text-white">IDE no Navegador</h1>
            <p className="text-gray-400 text-sm mt-1">
              Escreva e execute código C# diretamente no seu navegador
            </p>
          </div>
          <a
            href="/dashboard"
            className="px-4 py-2 bg-blue-600 text-white rounded-lg font-medium hover:bg-blue-700 transition-colors flex items-center gap-2"
          >
            <span>←</span>
            <span>Voltar ao Painel</span>
          </a>
        </div>
      </header>

      {/* Editor Container */}
      <div className="flex-1 overflow-hidden">
        <CodeEditor
          files={files}
          activeFileIndex={activeFileIndex}
          onFileChange={handleFileChange}
          onActiveFileChange={setActiveFileIndex}
          onAddFile={handleAddFile}
          onRemoveFile={handleRemoveFile}
          onRun={handleRun}
          output={getFormattedOutput()}
          isRunning={isRunning}
          executionStatus={status?.status}
          executionTime={executionTime}
          onGetFeedback={handleGetFeedback}
          showAIFeedback={showAIFeedback}
          aiFeedbackPanel={
            <AIFeedback
              suggestions={feedback?.suggestions || []}
              overallScore={feedback?.overallScore || 0}
              securityIssues={feedback?.securityIssues || []}
              performanceIssues={feedback?.performanceIssues || []}
              isLoading={isLoadingFeedback}
              onClose={handleCloseFeedback}
            />
          }
        />
      </div>
    </div>
  );
}
