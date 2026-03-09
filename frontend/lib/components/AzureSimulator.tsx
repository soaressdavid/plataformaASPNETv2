'use client';

import React, { useState } from 'react';
import { Icons } from './Icons';

export function AzureSimulator() {
  const [activeTab, setActiveTab] = useState<'resources' | 'deploy' | 'monitor'>('resources');

  const resources = [
    { name: 'myapp-webapp', type: 'App Service', status: 'Running', location: 'East US' },
    { name: 'myapp-sql', type: 'SQL Database', status: 'Online', location: 'East US' },
    { name: 'myapp-storage', type: 'Storage Account', status: 'Available', location: 'East US' },
  ];

  return (
    <div className="flex flex-col h-full bg-white">
      {/* Azure Header */}
      <div className="flex-shrink-0 bg-blue-600 text-white p-3">
        <div className="flex items-center gap-2">
          <div className="w-6 h-6 bg-white rounded flex items-center justify-center">
            <span className="text-blue-600 font-bold text-xs">Az</span>
          </div>
          <span className="font-semibold">Azure Portal</span>
        </div>
      </div>

      {/* Tabs */}
      <div className="flex-shrink-0 border-b border-gray-200 bg-gray-50">
        <div className="flex gap-1 p-2">
          {[
            { id: 'resources', label: 'Recursos', icon: <Icons.Folder className="w-4 h-4" /> },
            { id: 'deploy', label: 'Deploy', icon: <Icons.Rocket className="w-4 h-4" /> },
            { id: 'monitor', label: 'Monitor', icon: <Icons.ChartBar className="w-4 h-4" /> },
          ].map((tab) => (
            <button
              key={tab.id}
              onClick={() => setActiveTab(tab.id as any)}
              className={`px-4 py-2 text-sm font-medium rounded transition-colors flex items-center gap-2 ${
                activeTab === tab.id
                  ? 'bg-white text-blue-600 shadow'
                  : 'text-gray-600 hover:bg-gray-100'
              }`}
            >
              {tab.icon}
              {tab.label}
            </button>
          ))}
        </div>
      </div>

      {/* Content */}
      <div className="flex-1 overflow-auto p-4">
        {activeTab === 'resources' && (
          <div className="space-y-3">
            <h3 className="text-lg font-semibold text-gray-900 mb-4">Recursos do Azure</h3>
            {resources.map((resource, index) => (
              <div
                key={index}
                className="border border-gray-200 rounded-lg p-4 hover:shadow-md transition-shadow"
              >
                <div className="flex items-center justify-between mb-2">
                  <h4 className="font-semibold text-gray-900">{resource.name}</h4>
                  <span className="px-2 py-1 bg-green-100 text-green-800 text-xs rounded">
                    {resource.status}
                  </span>
                </div>
                <div className="text-sm text-gray-600 space-y-1">
                  <div>Tipo: {resource.type}</div>
                  <div>Localização: {resource.location}</div>
                </div>
              </div>
            ))}
          </div>
        )}

        {activeTab === 'deploy' && (
          <div className="space-y-4">
            <h3 className="text-lg font-semibold text-gray-900 mb-4">Deploy da Aplicação</h3>
            
            <div className="border border-gray-200 rounded-lg p-4">
              <h4 className="font-semibold mb-3">Configuração de Deploy</h4>
              <div className="space-y-3">
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    App Service
                  </label>
                  <select className="w-full border border-gray-300 rounded px-3 py-2 text-sm">
                    <option>myapp-webapp</option>
                  </select>
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    Método de Deploy
                  </label>
                  <select className="w-full border border-gray-300 rounded px-3 py-2 text-sm">
                    <option>GitHub Actions</option>
                    <option>Azure DevOps</option>
                    <option>FTP</option>
                  </select>
                </div>
                <button className="w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700 font-medium flex items-center justify-center gap-2">
                  <Icons.Rocket className="w-4 h-4" />
                  Fazer Deploy
                </button>
              </div>
            </div>

            <div className="bg-green-50 border border-green-200 rounded-lg p-4">
              <div className="flex items-start gap-2">
                <Icons.CheckCircle className="w-5 h-5 text-green-600 flex-shrink-0" />
                <div className="flex-1">
                  <div className="font-semibold text-green-900">Deploy Concluído</div>
                  <div className="text-sm text-green-700 mt-1">
                    Última atualização: há 5 minutos
                  </div>
                </div>
              </div>
            </div>
          </div>
        )}

        {activeTab === 'monitor' && (
          <div className="space-y-4">
            <h3 className="text-lg font-semibold text-gray-900 mb-4">Monitoramento</h3>
            
            <div className="grid grid-cols-2 gap-4">
              <div className="border border-gray-200 rounded-lg p-4">
                <div className="text-sm text-gray-600 mb-1">CPU</div>
                <div className="text-2xl font-bold text-blue-600">23%</div>
                <div className="mt-2 h-2 bg-gray-200 rounded-full overflow-hidden">
                  <div className="h-full bg-blue-600" style={{ width: '23%' }}></div>
                </div>
              </div>
              <div className="border border-gray-200 rounded-lg p-4">
                <div className="text-sm text-gray-600 mb-1">Memória</div>
                <div className="text-2xl font-bold text-green-600">45%</div>
                <div className="mt-2 h-2 bg-gray-200 rounded-full overflow-hidden">
                  <div className="h-full bg-green-600" style={{ width: '45%' }}></div>
                </div>
              </div>
            </div>

            <div className="border border-gray-200 rounded-lg p-4">
              <h4 className="font-semibold mb-3 flex items-center gap-2">
                <Icons.Activity className="w-4 h-4" />
                Requisições (últimas 24h)
              </h4>
              <div className="text-3xl font-bold text-purple-600">1,234</div>
              <div className="text-sm text-gray-600 mt-1 flex items-center gap-1">
                <Icons.ArrowUp className="w-3 h-3 text-green-600" />
                12% vs. ontem
              </div>
            </div>
          </div>
        )}
      </div>

      {/* Footer */}
      <div className="flex-shrink-0 p-3 bg-gray-50 border-t border-gray-200">
        <div className="text-xs text-gray-600 text-center flex items-center justify-center gap-2">
          <Icons.LightBulb className="w-3 h-3" />
          Simulador do Azure Portal para fins educacionais
        </div>
      </div>
    </div>
  );
}
