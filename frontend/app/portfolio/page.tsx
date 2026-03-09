'use client';

import { useState, useEffect } from 'react';
import { ProtectedRoute } from '@/lib/components/ProtectedRoute';
import { Navigation } from '@/lib/components/Navigation';
import { Icons } from '@/lib/components/Icons';
import apiClient from '@/lib/api-client';
import toast from 'react-hot-toast';
import { Prism as SyntaxHighlighter } from 'react-syntax-highlighter';
import { vscDarkPlus } from 'react-syntax-highlighter/dist/esm/styles/prism';

interface PortfolioProject {
  id: number;
  title: string;
  description: string;
  difficulty: 'Easy' | 'Medium' | 'Hard';
  category: string;
  completedAt: string;
  code: string;
  language: string;
  executionTime: number;
  testsPassed: number;
  totalTests: number;
  tags: string[];
}

interface Certificate {
  id: number;
  title: string;
  issuedAt: string;
  verificationCode: string;
  pdfUrl: string;
}

interface PortfolioStats {
  totalProjects: number;
  totalChallenges: number;
  totalXP: number;
  certificatesEarned: number;
  averageExecutionTime: number;
  successRate: number;
}

function PortfolioContent() {
  const [projects, setProjects] = useState<PortfolioProject[]>([]);
  const [certificates, setCertificates] = useState<Certificate[]>([]);
  const [stats, setStats] = useState<PortfolioStats | null>(null);
  const [loading, setLoading] = useState(true);
  const [selectedDifficulty, setSelectedDifficulty] = useState<string>('All');
  const [selectedCategory, setSelectedCategory] = useState<string>('All');
  const [expandedProject, setExpandedProject] = useState<number | null>(null);

  useEffect(() => {
    fetchPortfolioData();
  }, []);

  const fetchPortfolioData = async () => {
    try {
      setLoading(true);
      
      // Fetch portfolio stats
      const statsResponse = await apiClient.get('/api/portfolio/stats');
      setStats(statsResponse.data);

      // Fetch completed projects
      const projectsResponse = await apiClient.get('/api/portfolio/projects');

      setProjects(projectsResponse.data);

      // Fetch certificates
      const certificatesResponse = await apiClient.get('/api/portfolio/certificates');
      setCertificates(certificatesResponse.data);
    } catch (error) {
      console.error('Error fetching portfolio:', error);
      toast.error('Failed to load portfolio data');
    } finally {
      setLoading(false);
    }
  };

  const downloadCertificate = async (certificateId: number) => {
    try {
      const response = await apiClient.get(`/api/portfolio/certificates/${certificateId}/download`, {
        responseType: 'blob'
      });
      
      const url = window.URL.createObjectURL(new Blob([response.data]));
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', `certificate-${certificateId}.pdf`);
      document.body.appendChild(link);
      link.click();
      link.remove();
      
      toast.success('Certificate downloaded successfully!');
    } catch (error) {
      console.error('Error downloading certificate:', error);
      toast.error('Failed to download certificate');
    }
  };

  const filteredProjects = projects.filter(project => {
    const difficultyMatch = selectedDifficulty === 'All' || project.difficulty === selectedDifficulty;
    const categoryMatch = selectedCategory === 'All' || project.category === selectedCategory;
    return difficultyMatch && categoryMatch;
  });

  const categories = ['All', ...Array.from(new Set(projects.map(p => p.category)))];
  const difficulties = ['All', 'Easy', 'Medium', 'Hard'];

  const getDifficultyColor = (difficulty: string) => {
    switch (difficulty) {
      case 'Easy': return 'text-green-600 bg-green-100';
      case 'Medium': return 'text-yellow-600 bg-yellow-100';
      case 'Hard': return 'text-red-600 bg-red-100';
      default: return 'text-gray-600 bg-gray-100';
    }
  };

  if (loading) {
    return (
      <div className="min-h-screen bg-gray-50">
        <Navigation />
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
          <div className="flex items-center justify-center h-64">
            <div className="text-center">
              <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-primary-600 mx-auto"></div>
              <p className="mt-4 text-gray-600">Loading portfolio...</p>
            </div>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <Navigation />
      
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {/* Header */}
        <div className="mb-8">
          <h1 className="text-3xl font-bold text-gray-900">My Portfolio</h1>
          <p className="mt-2 text-gray-600">Showcase of completed projects and achievements</p>
        </div>

        {/* Stats Cards */}
        {stats && (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 mb-8">
            <div className="bg-white rounded-lg shadow p-6">
              <div className="flex items-center justify-between">
                <div>
                  <p className="text-sm text-gray-600">Total Projects</p>
                  <p className="text-2xl font-bold text-gray-900">{stats.totalProjects}</p>
                </div>
                <Icons.Folder className="h-8 w-8 text-primary-600" />
              </div>
            </div>

            <div className="bg-white rounded-lg shadow p-6">
              <div className="flex items-center justify-between">
                <div>
                  <p className="text-sm text-gray-600">Challenges Completed</p>
                  <p className="text-2xl font-bold text-gray-900">{stats.totalChallenges}</p>
                </div>
                <Icons.Award className="h-8 w-8 text-yellow-600" />
              </div>
            </div>

            <div className="bg-white rounded-lg shadow p-6">
              <div className="flex items-center justify-between">
                <div>
                  <p className="text-sm text-gray-600">Total XP Earned</p>
                  <p className="text-2xl font-bold text-gray-900">{stats.totalXP.toLocaleString()}</p>
                </div>
                <Icons.Star className="h-8 w-8 text-purple-600" />
              </div>
            </div>

            <div className="bg-white rounded-lg shadow p-6">
              <div className="flex items-center justify-between">
                <div>
                  <p className="text-sm text-gray-600">Certificates</p>
                  <p className="text-2xl font-bold text-gray-900">{stats.certificatesEarned}</p>
                </div>
                <Icons.Award className="h-8 w-8 text-green-600" />
              </div>
            </div>

            <div className="bg-white rounded-lg shadow p-6">
              <div className="flex items-center justify-between">
                <div>
                  <p className="text-sm text-gray-600">Avg Execution Time</p>
                  <p className="text-2xl font-bold text-gray-900">{stats.averageExecutionTime}ms</p>
                </div>
                <Icons.Clock className="h-8 w-8 text-blue-600" />
              </div>
            </div>

            <div className="bg-white rounded-lg shadow p-6">
              <div className="flex items-center justify-between">
                <div>
                  <p className="text-sm text-gray-600">Success Rate</p>
                  <p className="text-2xl font-bold text-gray-900">{stats.successRate}%</p>
                </div>
                <Icons.CheckCircle className="h-8 w-8 text-green-600" />
              </div>
            </div>
          </div>
        )}

        {/* Filters */}
        <div className="bg-white rounded-lg shadow p-6 mb-8">
          <div className="flex flex-wrap gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">Difficulty</label>
              <select
                value={selectedDifficulty}
                onChange={(e) => setSelectedDifficulty(e.target.value)}
                className="block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-primary-500 focus:border-primary-500"
              >
                {difficulties.map(diff => (
                  <option key={diff} value={diff}>{diff}</option>
                ))}
              </select>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">Category</label>
              <select
                value={selectedCategory}
                onChange={(e) => setSelectedCategory(e.target.value)}
                className="block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-primary-500 focus:border-primary-500"
              >
                {categories.map(cat => (
                  <option key={cat} value={cat}>{cat}</option>
                ))}
              </select>
            </div>
          </div>
        </div>

        {/* Projects Grid */}
        <div className="mb-12">
          <h2 className="text-2xl font-bold text-gray-900 mb-6">Completed Projects</h2>
          
          {filteredProjects.length === 0 ? (
            <div className="bg-white rounded-lg shadow p-12 text-center">
              <Icons.Folder className="h-16 w-16 text-gray-400 mx-auto mb-4" />
              <p className="text-gray-600">No projects found matching your filters</p>
            </div>
          ) : (
            <div className="grid grid-cols-1 gap-6">
              {filteredProjects.map(project => (
                <div key={project.id} className="bg-white rounded-lg shadow hover:shadow-lg transition-shadow">
                  <div className="p-6">
                    <div className="flex items-start justify-between mb-4">
                      <div className="flex-1">
                        <h3 className="text-xl font-bold text-gray-900 mb-2">{project.title}</h3>
                        <p className="text-gray-600 mb-4">{project.description}</p>
                        
                        <div className="flex flex-wrap gap-2 mb-4">
                          <span className={`px-3 py-1 rounded-full text-sm font-medium ${getDifficultyColor(project.difficulty)}`}>
                            {project.difficulty}
                          </span>
                          <span className="px-3 py-1 rounded-full text-sm font-medium text-blue-600 bg-blue-100">
                            {project.category}
                          </span>
                          {project.tags.map(tag => (
                            <span key={tag} className="px-3 py-1 rounded-full text-sm font-medium text-gray-600 bg-gray-100">
                              {tag}
                            </span>
                          ))}
                        </div>

                        <div className="flex items-center gap-6 text-sm text-gray-600">
                          <div className="flex items-center gap-1">
                            <Icons.Clock className="h-4 w-4" />
                            <span>{project.executionTime}ms</span>
                          </div>
                          <div className="flex items-center gap-1">
                            <Icons.CheckCircle className="h-4 w-4 text-green-600" />
                            <span>{project.testsPassed}/{project.totalTests} tests passed</span>
                          </div>
                          <div className="flex items-center gap-1">
                            <Icons.Calendar className="h-4 w-4" />
                            <span>Completed {new Date(project.completedAt).toLocaleDateString()}</span>
                          </div>
                        </div>
                      </div>
                    </div>

                    {/* Code Sample */}
                    <div className="mt-4">
                      <button
                        onClick={() => setExpandedProject(expandedProject === project.id ? null : project.id)}
                        className="flex items-center gap-2 text-primary-600 hover:text-primary-700 font-medium"
                      >
                        <Icons.Code className="h-5 w-5" />
                        <span>{expandedProject === project.id ? 'Hide' : 'View'} Code</span>
                        <Icons.ChevronDown className={`h-4 w-4 transition-transform ${expandedProject === project.id ? 'rotate-180' : ''}`} />
                      </button>

                      {expandedProject === project.id && (
                        <div className="mt-4 rounded-lg overflow-hidden">
                          <SyntaxHighlighter
                            language={project.language.toLowerCase()}
                            style={vscDarkPlus}
                            customStyle={{
                              margin: 0,
                              borderRadius: '0.5rem',
                              fontSize: '0.875rem'
                            }}
                          >
                            {project.code}
                          </SyntaxHighlighter>
                        </div>
                      )}
                    </div>
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>

        {/* Certificates Section */}
        {certificates.length > 0 && (
          <div>
            <h2 className="text-2xl font-bold text-gray-900 mb-6">Certificates</h2>
            
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
              {certificates.map(cert => (
                <div key={cert.id} className="bg-white rounded-lg shadow hover:shadow-lg transition-shadow p-6">
                  <div className="flex items-start justify-between mb-4">
                    <Icons.Award className="h-12 w-12 text-yellow-600" />
                    <button
                      onClick={() => downloadCertificate(cert.id)}
                      className="text-primary-600 hover:text-primary-700"
                      title="Download Certificate"
                    >
                      <Icons.Download className="h-5 w-5" />
                    </button>
                  </div>
                  
                  <h3 className="text-lg font-bold text-gray-900 mb-2">{cert.title}</h3>
                  
                  <div className="space-y-2 text-sm text-gray-600">
                    <div className="flex items-center gap-2">
                      <Icons.Calendar className="h-4 w-4" />
                      <span>Issued {new Date(cert.issuedAt).toLocaleDateString()}</span>
                    </div>
                    <div className="flex items-center gap-2">
                      <Icons.CheckCircle className="h-4 w-4" />
                      <span className="font-mono text-xs">{cert.verificationCode}</span>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          </div>
        )}
      </div>
    </div>
  );
}

export default function PortfolioPage() {
  return (
    <ProtectedRoute>
      <PortfolioContent />
    </ProtectedRoute>
  );
}
