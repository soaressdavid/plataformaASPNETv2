'use client';

import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import Link from 'next/link';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { MessageSquare, Eye, Pin, Lock, CheckCircle, ArrowLeft } from 'lucide-react';

interface Thread {
  id: string;
  title: string;
  authorName: string;
  authorId: string;
  postCount: number;
  viewCount: number;
  isPinned: boolean;
  isLocked: boolean;
  hasAcceptedAnswer: boolean;
  createdAt: string;
  updatedAt: string;
}

interface PagedResult {
  items: Thread[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

export default function CategoryPage() {
  const params = useParams();
  const router = useRouter();
  const category = decodeURIComponent(params.category as string);
  
  const [threads, setThreads] = useState<Thread[]>([]);
  const [loading, setLoading] = useState(true);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);

  useEffect(() => {
    fetchThreads();
  }, [category, page]);

  const fetchThreads = async () => {
    try {
      const response = await fetch(
        `http://localhost:5007/api/forum/categories/${encodeURIComponent(category)}/threads?page=${page}&pageSize=20`
      );
      if (response.ok) {
        const data: PagedResult = await response.json();
        setThreads(data.items);
        setTotalPages(data.totalPages);
      }
    } catch (error) {
      console.error('Error fetching threads:', error);
    } finally {
      setLoading(false);
    }
  };

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    const now = new Date();
    const diffMs = now.getTime() - date.getTime();
    const diffMins = Math.floor(diffMs / 60000);
    const diffHours = Math.floor(diffMs / 3600000);
    const diffDays = Math.floor(diffMs / 86400000);

    if (diffMins < 60) return `${diffMins}m ago`;
    if (diffHours < 24) return `${diffHours}h ago`;
    if (diffDays < 7) return `${diffDays}d ago`;
    return date.toLocaleDateString();
  };

  if (loading) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="flex items-center justify-center min-h-[400px]">
          <div className="text-center">
            <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto"></div>
            <p className="mt-4 text-gray-600">Loading threads...</p>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="mb-6">
        <Button variant="ghost" asChild className="mb-4">
          <Link href="/forum">
            <ArrowLeft className="h-4 w-4 mr-2" />
            Back to Forum
          </Link>
        </Button>
        
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-4xl font-bold mb-2">{category}</h1>
            <p className="text-gray-600">
              {threads.length} thread{threads.length !== 1 ? 's' : ''}
            </p>
          </div>
          <Button asChild>
            <Link href={`/forum/new?category=${encodeURIComponent(category)}`}>
              Create Thread
            </Link>
          </Button>
        </div>
      </div>

      {threads.length === 0 ? (
        <Card>
          <CardContent className="py-12 text-center">
            <MessageSquare className="h-12 w-12 text-gray-400 mx-auto mb-4" />
            <h3 className="text-lg font-semibold mb-2">No threads yet</h3>
            <p className="text-gray-600 mb-4">
              Be the first to start a discussion in this category!
            </p>
            <Button asChild>
              <Link href={`/forum/new?category=${encodeURIComponent(category)}`}>
                Create Thread
              </Link>
            </Button>
          </CardContent>
        </Card>
      ) : (
        <>
          <div className="space-y-3">
            {threads.map((thread) => (
              <Link key={thread.id} href={`/forum/thread/${thread.id}`}>
                <Card className="hover:shadow-md transition-shadow cursor-pointer">
                  <CardHeader className="pb-3">
                    <div className="flex items-start justify-between gap-4">
                      <div className="flex-1 min-w-0">
                        <div className="flex items-center gap-2 mb-2">
                          {thread.isPinned && (
                            <Pin className="h-4 w-4 text-blue-600 flex-shrink-0" />
                          )}
                          {thread.isLocked && (
                            <Lock className="h-4 w-4 text-gray-500 flex-shrink-0" />
                          )}
                          {thread.hasAcceptedAnswer && (
                            <CheckCircle className="h-4 w-4 text-green-600 flex-shrink-0" />
                          )}
                          <CardTitle className="text-lg truncate">
                            {thread.title}
                          </CardTitle>
                        </div>
                        <CardDescription className="flex items-center gap-3 text-sm flex-wrap">
                          <span>by {thread.authorName}</span>
                          <span className="flex items-center gap-1">
                            <MessageSquare className="h-3 w-3" />
                            {thread.postCount}
                          </span>
                          <span className="flex items-center gap-1">
                            <Eye className="h-3 w-3" />
                            {thread.viewCount}
                          </span>
                          <span>{formatDate(thread.updatedAt)}</span>
                        </CardDescription>
                      </div>
                    </div>
                  </CardHeader>
                </Card>
              </Link>
            ))}
          </div>

          {totalPages > 1 && (
            <div className="flex items-center justify-center gap-2 mt-6">
              <Button
                variant="outline"
                onClick={() => setPage(p => Math.max(1, p - 1))}
                disabled={page === 1}
              >
                Previous
              </Button>
              <span className="text-sm text-gray-600">
                Page {page} of {totalPages}
              </span>
              <Button
                variant="outline"
                onClick={() => setPage(p => Math.min(totalPages, p + 1))}
                disabled={page === totalPages}
              >
                Next
              </Button>
            </div>
          )}
        </>
      )}
    </div>
  );
}
