'use client';

import React, { useState } from 'react';
import { Icons } from './Icons';
import toast from 'react-hot-toast';

interface QueryResult {
  success: boolean;
  columns?: string[];
  rows?: any[][];
  rowsAffected?: number;
  message?: string;
  error?: string;
  executionTime?: number;
}

interface DatabaseTable {
  name: string;
  schema: string;
  columns: { name: string; type: string; nullable: boolean }[];
}

export function SqlExecutor() {
  const [sqlQuery, setSqlQuery] = useState('-- Escreva sua consulta SQL aqui\nSELECT TOP 10 * FROM Users;');
  const [result, setResult] = useState<QueryResult | null>(null);
  const [loading, setLoading] = useState(false);
  const [activeTab, setActiveTab] = useState<'query' | 'results' | 'messages'>('query');
  const [showObjectExplorer, setShowObjectExplorer] = useState(true);
  const [expandedTables, setExpandedTables] = useState<Set<string>>(new Set());

  // Tabelas disponíveis no banco
  const tables: DatabaseTable[] = [
    {
      name: 'Users',
      schema: 'dbo',
      columns: [
        { name: 'Id', type: 'uniqueidentifier', nullable: false },
        { name: 'Name', type: 'nvarchar(100)', nullable: false },
        { name: 'Email', type: 'nvarchar(255)', nullable: false },
        { name: 'PasswordHash', type: 'nvarchar(max)', nullable: false },
        { name: 'CreatedAt', type: 'datetime2', nullable: false },
        { name: 'IsActive', type: 'bit', nullable: false },
      ]
    },
    {
      name: 'Courses',
      schema: 'dbo',
      columns: [
        { name: 'Id', type: 'uniqueidentifier', nullable: false },
        { name: 'Title', type: 'nvarchar(200)', nullable: false },
        { name: 'Description', type: 'nvarchar(max)', nullable: true },
        { name: 'Level', type: 'int', nullable: false },
        { name: 'CreatedAt', type: 'datetime2', nullable: false },
      ]
    },
    {
      name: 'Lessons',
      schema: 'dbo',
      columns: [
        { name: 'Id', type: 'uniqueidentifier', nullable: false },
        { name: 'CourseId', type: 'uniqueidentifier', nullable: false },
        { name: 'Title', type: 'nvarchar(200)', nullable: false },
        { name: 'Content', type: 'nvarchar(max)', nullable: true },
        { name: 'Order', type: 'int', nullable: false },
      ]
    },
    {
      name: 'Enrollments',
      schema: 'dbo',
      columns: [
        { name: 'Id', type: 'uniqueidentifier', nullable: false },
        { name: 'UserId', type: 'uniqueidentifier', nullable: false },
        { name: 'CourseId', type: 'uniqueidentifier', nullable: false },
        { name: 'EnrolledAt', type: 'datetime2', nullable: false },
        { name: 'CompletedAt', type: 'datetime2', nullable: true },
      ]
    },
  ];

  const exampleQueries = [
    { label: 'Todos Usuários', query: 'SELECT TOP 10 * FROM Users;' },
    { label: 'Usuários Ativos', query: 'SELECT Id, Name, Email, CreatedAt\nFROM Users\nWHERE IsActive = 1\nORDER BY CreatedAt DESC;' },
    { label: 'Cursos por Nível', query: 'SELECT Level, COUNT(*) as TotalCursos\nFROM Courses\nGROUP BY Level\nORDER BY Level;' },
    { label: 'JOIN Completo', query: 'SELECT TOP 10\n  u.Name as Usuario,\n  c.Title as Curso,\n  e.EnrolledAt as DataMatricula\nFROM Users u\nINNER JOIN Enrollments e ON u.Id = e.UserId\nINNER JOIN Courses c ON e.CourseId = c.Id\nORDER BY e.EnrolledAt DESC;' },
  ];

  const toggleTable = (tableName: string) => {
    const newExpanded = new Set(expandedTables);
    if (newExpanded.has(tableName)) {
      newExpanded.delete(tableName);
    } else {
      newExpanded.add(tableName);
    }
    setExpandedTables(newExpanded);
  };

  const insertTableName = (tableName: string) => {
    setSqlQuery(prev => prev + (prev.endsWith('\n') || prev.endsWith(' ') ? '' : ' ') + tableName);
  };

  const handleExecute = async () => {
    if (!sqlQuery.trim()) {
      toast.error('Digite uma consulta SQL');
      return;
    }

    try {
      setLoading(true);
      setResult(null);

      const response = await fetch('http://localhost:5008/api/sql/execute', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          Query: sqlQuery
        })
      });

      const data = await response.json();
      
      // Adaptar resposta do SqlExecutor REAL para o formato esperado
      if (data.success) {
        if (data.data && Array.isArray(data.data)) {
          // SELECT query - converter para formato de tabela
          const adaptedResult = {
            success: true,
            columns: data.data.length > 0 ? Object.keys(data.data[0]) : [],
            rows: data.data.map(row => Object.values(row)),
            rowsAffected: data.rowsAffected,
            message: data.message,
            executionTime: data.executionTimeMs
          };
          setResult(adaptedResult);
        } else {
          // INSERT/UPDATE/DELETE query
          setResult({
            success: true,
            rowsAffected: data.rowsAffected,
            message: data.message,
            executionTime: data.executionTimeMs
          });
        }
      } else {
        setResult({
          success: false,
          error: data.error
        });
      }

      if (data.success) {
        toast.success('Consulta executada!');
      } else {
        toast.error('Erro na consulta');
      }
    } catch (error) {
      console.error('Error executing SQL:', error);
      toast.error('Erro ao conectar com o servidor');
      setResult({
        success: false,
        error: 'Erro ao conectar com o servidor SQL'
      });
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="flex flex-col h-full">
      {/* Header com exemplos */}
      <div className="flex-shrink-0 p-4 border-b border-gray-200 bg-gray-50">
        <div className="flex items-center justify-between mb-3">
          <h3 className="text-sm font-semibold text-gray-900">Exemplos Rápidos:</h3>
        </div>
        <div className="flex flex-wrap gap-2">
          {exampleQueries.map((example, index) => (
            <button
              key={index}
              onClick={() => setSqlQuery(example.query)}
              className="px-3 py-1 text-xs font-medium text-gray-700 bg-white border border-gray-300 rounded-lg hover:bg-blue-50 hover:border-blue-400 hover:text-blue-700 transition-colors"
            >
              {example.label}
            </button>
          ))}
        </div>
      </div>

      {/* Editor SQL */}
      <div className="flex-1 flex flex-col min-h-0">
        <div className="flex-shrink-0 p-3 bg-gray-100 border-b border-gray-200">
          <div className="flex items-center justify-between">
            <span className="text-xs font-medium text-gray-900">Editor SQL</span>
            <div className="flex gap-2">
              <button
                onClick={() => setSqlQuery('')}
                className="px-3 py-1 text-xs text-gray-700 bg-white border border-gray-300 rounded hover:bg-gray-50 font-medium"
              >
                Limpar
              </button>
              <button
                onClick={handleExecute}
                disabled={loading}
                className="px-4 py-1 text-xs font-medium text-white bg-blue-600 rounded hover:bg-blue-700 disabled:bg-gray-400 disabled:cursor-not-allowed flex items-center gap-1"
              >
                {loading ? (
                  <>
                    <svg className="animate-spin h-3 w-3" fill="none" viewBox="0 0 24 24">
                      <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                      <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                    </svg>
                    Executando...
                  </>
                ) : (
                  <>
                    <Icons.Play className="w-3 h-3" />
                    Executar
                  </>
                )}
              </button>
            </div>
          </div>
        </div>

        <textarea
          value={sqlQuery}
          onChange={(e) => setSqlQuery(e.target.value)}
          className="flex-1 p-4 font-mono text-sm text-gray-900 bg-white border-0 focus:ring-0 resize-none placeholder:text-gray-400"
          placeholder="Digite sua consulta SQL aqui..."
          spellCheck={false}
        />
      </div>

      {/* Resultados */}
      {result && (
        <div className="flex-shrink-0 border-t border-gray-200 bg-white max-h-64 overflow-auto">
          <div className="p-3 bg-gray-50 border-b border-gray-200">
            <h3 className="text-xs font-semibold text-gray-900 flex items-center gap-2">
              {result.success ? (
                <>
                  <Icons.CheckCircle className="w-4 h-4 text-green-600" />
                  Resultado
                </>
              ) : (
                <>
                  <Icons.XCircle className="w-4 h-4 text-red-600" />
                  Erro
                </>
              )}
            </h3>
          </div>
          <div className="p-3">
            {result.success ? (
              <>
                {result.columns && result.rows && result.rows.length > 0 ? (
                  <div className="overflow-x-auto">
                    <table className="min-w-full text-xs">
                      <thead className="bg-gray-100">
                        <tr>
                          {result.columns.map((col, index) => (
                            <th
                              key={index}
                              className="px-3 py-2 text-left font-semibold text-gray-900 bg-gray-50"
                            >
                              {col}
                            </th>
                          ))}
                        </tr>
                      </thead>
                      <tbody className="divide-y divide-gray-200">
                        {result.rows.map((row, rowIndex) => (
                          <tr key={rowIndex} className="hover:bg-gray-50">
                            {row.map((cell, cellIndex) => (
                              <td
                                key={cellIndex}
                                className="px-3 py-2 text-gray-900"
                              >
                                {cell === null ? (
                                  <span className="text-gray-400 italic">NULL</span>
                                ) : (
                                  String(cell)
                                )}
                              </td>
                            ))}
                          </tr>
                        ))}
                      </tbody>
                    </table>
                    <div className="mt-2 text-xs text-gray-700 font-medium flex items-center gap-2">
                      <Icons.Table className="w-3 h-3" />
                      {result.rows.length} linha(s)
                      {result.executionTime && (
                        <>
                          <span className="mx-1">•</span>
                          <Icons.Clock className="w-3 h-3" />
                          {result.executionTime}ms
                        </>
                      )}
                    </div>
                  </div>
                ) : (
                  <div className="text-center py-4">
                    <p className="text-sm text-gray-700 font-medium flex items-center justify-center gap-2">
                      <Icons.CheckCircle className="w-4 h-4 text-green-600" />
                      {result.message || 'Consulta executada com sucesso'}
                    </p>
                    {result.rowsAffected !== undefined && (
                      <p className="text-xs text-gray-600 mt-1">
                        {result.rowsAffected} linha(s) afetada(s)
                      </p>
                    )}
                  </div>
                )}
              </>
            ) : (
              <div className="bg-red-50 border border-red-200 rounded p-3">
                <p className="text-xs text-red-800 font-mono whitespace-pre-wrap">
                  {result.error}
                </p>
              </div>
            )}
          </div>
        </div>
      )}

      {/* Schema Info */}
      <div className="flex-shrink-0 p-3 border-t border-gray-200 bg-blue-50">
        <details className="text-xs">
          <summary className="cursor-pointer font-semibold text-blue-900 mb-2 hover:text-blue-700 flex items-center gap-2">
            <Icons.Table className="w-4 h-4" />
            Tabelas Disponíveis
          </summary>
          <div className="space-y-1 mt-2">
            <div className="p-2 bg-white rounded text-xs border border-blue-100">
              <div className="font-semibold text-gray-900">Users</div>
              <div className="text-gray-700">Id, Name, Email, CreatedAt</div>
            </div>
            <div className="p-2 bg-white rounded text-xs border border-blue-100">
              <div className="font-semibold text-gray-900">Courses</div>
              <div className="text-gray-700">Id, Title, Description, Level</div>
            </div>
            <div className="p-2 bg-white rounded text-xs border border-blue-100">
              <div className="font-semibold text-gray-900">Lessons</div>
              <div className="text-gray-700">Id, CourseId, Title, Content</div>
            </div>
          </div>
        </details>
      </div>
    </div>
  );
}
