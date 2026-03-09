'use client';

import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import Link from 'next/link';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Textarea } from '@/components/ui/textarea';
import { Badge } from '@/components/ui/badge';
import { Avatar, AvatarFallback } from '@/components/ui/avatar';
import { 
  ArrowLeft, 
  ThumbsUp, 
  CheckCircle, 
  Eye, 
  MessageSquare,
  Award,
  Edit,
  Flag
} from 'lucide-react';
import { useToast } from '@/hooks/use-toast';

interface Post {
  id: string;
  content: string;
  authorId: string;
  authorName: string;
  authorLevel: number;
  authorXP: number;
  upvotes: number;
  isEdited: boolean;
  lastEditedAt?: string;
  isAcceptedAnswer: boolean;
  createdAt: string;
}

interface ThreadDetail {
  id: string;
  title: string;
  content: string;
  authorId: string;
  authorName: string;
  authorLevel: number;
  authorXP: number;
  challengeId?: string;
  challengeName?: string;
  lessonId?: string;
  lessonName?: string;
  category?: string;
  viewCount: number;
  isPinned: boolean;
  isLocked: boolean;
  acceptedAnswerId?: string;
  createdAt: string;
  updatedAt: string;
  posts: Post[];
}

export default function ThreadPage() {
  const params = useParams();
  const router = useRouter();
  const { toast } = useToast();
  const threadId = params.id as string;
  
  const [thread, setThread] = useState<ThreadDetail | null>(null);
  const [loading, setLoading] = useState(true);
  const [replyContent, setReplyContent] = useState('');
  const [submitting, setSubmitting] = useState(false);
  
  // Mock user ID - in production, get from auth context
  const currentUserId = 'user-id-placeholder';

  useEffect(() => {
    fetchThread();
  }, [threadId]);

  const fetchThread = async () => {
    try {
      const response = await fetch(`http://localhost:5007/api/forum/threads/${threadId}`);
      if (response.ok) {
        const data = await response.json();
        setThread(data);
      } else {
        toast({
          title: 'Error',
          description: 'Failed to load thread',
          variant: 'destructive'
        });
      }
    } catch (error) {
      console.error('Error fetching thread:', error);
      toast({
        title: 'Error',
        description: 'Failed to load thread',
        variant: 'destructive'
      });
    } finally {
      setLoading(false);
    }
  };

  const handleReply = async () => {
    if (!replyContent.trim()) {
      toast({
        title: 'Error',
        description: 'Please enter a reply',
        variant: 'destructive'
      });
      return;
    }

    if (thread?.isLocked) {
      toast({
        title: 'Error',
        description: 'This thread is locked',
        variant: 'destructive'
      });
      return;
    }

    setSubmitting(true);
    try {
      const response = await fetch(`http://localhost:5007/api/forum/threads/${threadId}/posts`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          content: replyContent,
          authorId: currentUserId
        })
      });

      if (response.ok) {
        toast({
          title: 'Success',
          description: 'Reply posted successfully'
        });
        setReplyContent('');
        fetchThread(); // Refresh thread
      } else {
        toast({
          title: 'Error',
          description: 'Failed to post reply',
          variant: 'destructive'
        });
      }
    } catch (error) {
      console.error('Error posting reply:', error);
      toast({
        title: 'Error',
        description: 'Failed to post reply',
        variant: 'destructive'
      });
    } finally {
      setSubmitting(false);
    }
  };

  const handleUpvote = async (postId: string) => {
    try {
      const response = await fetch(`http://localhost:5007/api/forum/posts/${postId}/upvote`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ userId: currentUserId })
      });

      if (response.ok) {
        fetchThread(); // Refresh to show updated vote count
      }
    } catch (error) {
      console.error('Error upvoting post:', error);
    }
  };

  const handleAcceptAnswer = async (postId: string) => {
    try {
      const response = await fetch(
        `http://localhost:5007/api/forum/threads/${threadId}/accept/${postId}`,
        {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ userId: currentUserId })
        }
      );

      if (response.ok) {
        toast({
          title: 'Success',
          description: 'Answer marked as accepted'
        });
        fetchThread();
      } else {
        toast({
          title: 'Error',
          description: 'Failed to accept answer',
          variant: 'destructive'
        });
      }
    } catch (error) {
      console.error('Error accepting answer:', error);
    }
  };

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return date.toLocaleString();
  };

  const getInitials = (name: string) => {
    return name
      .split(' ')
      .map(n => n[0])
      .join('')
      .toUpperCase()
      .slice(0, 2);
  };

  if (loading) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="flex items-center justify-center min-h-[400px]">
          <div className="text-center">
            <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto"></div>
            <p className="mt-4 text-gray-600">Loading thread...</p>
          </div>
        </div>
      </div>
    );
  }

  if (!thread) {
    return (
      <div className="container mx-auto px-4 py-8">
        <Card>
          <CardContent className="py-12 text-center">
            <h3 className="text-lg font-semibold mb-2">Thread not found</h3>
            <Button asChild>
              <Link href="/forum">Back to Forum</Link>
            </Button>
          </CardContent>
        </Card>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8 max-w-4xl">
      <Button variant="ghost" asChild className="mb-4">
        <Link href={thread.category ? `/forum/category/${encodeURIComponent(thread.category)}` : '/forum'}>
          <ArrowLeft className="h-4 w-4 mr-2" />
          Back
        </Link>
      </Button>

      {/* Thread Header */}
      <Card className="mb-6">
        <CardHeader>
          <div className="flex items-start justify-between gap-4">
            <div className="flex-1">
              <CardTitle className="text-2xl mb-3">{thread.title}</CardTitle>
              <div className="flex items-center gap-4 text-sm text-gray-600 mb-4">
                <span className="flex items-center gap-1">
                  <Eye className="h-4 w-4" />
                  {thread.viewCount} views
                </span>
                <span className="flex items-center gap-1">
                  <MessageSquare className="h-4 w-4" />
                  {thread.posts.length} replies
                </span>
              </div>
              {thread.category && (
                <Badge variant="secondary">{thread.category}</Badge>
              )}
              {thread.challengeName && (
                <Badge variant="outline" className="ml-2">
                  Challenge: {thread.challengeName}
                </Badge>
              )}
              {thread.lessonName && (
                <Badge variant="outline" className="ml-2">
                  Lesson: {thread.lessonName}
                </Badge>
              )}
            </div>
          </div>
        </CardHeader>
        <CardContent>
          <div className="flex gap-4">
            <Avatar className="h-12 w-12">
              <AvatarFallback>{getInitials(thread.authorName)}</AvatarFallback>
            </Avatar>
            <div className="flex-1">
              <div className="flex items-center gap-2 mb-2">
                <span className="font-semibold">{thread.authorName}</span>
                <Badge variant="outline" className="text-xs">
                  Level {thread.authorLevel}
                </Badge>
                <span className="text-xs text-gray-500">
                  {thread.authorXP} XP
                </span>
              </div>
              <div className="prose max-w-none mb-3">
                <p className="whitespace-pre-wrap">{thread.content}</p>
              </div>
              <div className="text-xs text-gray-500">
                Posted {formatDate(thread.createdAt)}
              </div>
            </div>
          </div>
        </CardContent>
      </Card>

      {/* Posts/Replies */}
      <div className="space-y-4 mb-6">
        {thread.posts.map((post) => (
          <Card key={post.id} className={post.isAcceptedAnswer ? 'border-green-500 border-2' : ''}>
            <CardContent className="pt-6">
              <div className="flex gap-4">
                <div className="flex flex-col items-center gap-2">
                  <Avatar className="h-10 w-10">
                    <AvatarFallback>{getInitials(post.authorName)}</AvatarFallback>
                  </Avatar>
                  <Button
                    variant="ghost"
                    size="sm"
                    onClick={() => handleUpvote(post.id)}
                    className="flex flex-col items-center p-2 h-auto"
                  >
                    <ThumbsUp className="h-4 w-4" />
                    <span className="text-xs">{post.upvotes}</span>
                  </Button>
                </div>
                <div className="flex-1">
                  <div className="flex items-center gap-2 mb-2">
                    <span className="font-semibold">{post.authorName}</span>
                    <Badge variant="outline" className="text-xs">
                      Level {post.authorLevel}
                    </Badge>
                    {post.isAcceptedAnswer && (
                      <Badge className="bg-green-600 text-xs">
                        <CheckCircle className="h-3 w-3 mr-1" />
                        Accepted Answer
                      </Badge>
                    )}
                  </div>
                  <div className="prose max-w-none mb-3">
                    <p className="whitespace-pre-wrap">{post.content}</p>
                  </div>
                  <div className="flex items-center gap-3 text-xs text-gray-500">
                    <span>{formatDate(post.createdAt)}</span>
                    {post.isEdited && post.lastEditedAt && (
                      <span>(edited {formatDate(post.lastEditedAt)})</span>
                    )}
                    {!post.isAcceptedAnswer && thread.authorId === currentUserId && (
                      <Button
                        variant="ghost"
                        size="sm"
                        onClick={() => handleAcceptAnswer(post.id)}
                        className="h-auto py-1 px-2"
                      >
                        <CheckCircle className="h-3 w-3 mr-1" />
                        Accept Answer
                      </Button>
                    )}
                  </div>
                </div>
              </div>
            </CardContent>
          </Card>
        ))}
      </div>

      {/* Reply Form */}
      {!thread.isLocked ? (
        <Card>
          <CardHeader>
            <CardTitle className="text-lg">Post a Reply</CardTitle>
          </CardHeader>
          <CardContent>
            <Textarea
              placeholder="Write your reply here... (Markdown supported)"
              value={replyContent}
              onChange={(e) => setReplyContent(e.target.value)}
              rows={6}
              className="mb-4"
            />
            <div className="flex justify-between items-center">
              <p className="text-sm text-gray-600">
                Helpful posts earn XP when they receive upvotes!
              </p>
              <Button onClick={handleReply} disabled={submitting}>
                {submitting ? 'Posting...' : 'Post Reply'}
              </Button>
            </div>
          </CardContent>
        </Card>
      ) : (
        <Card>
          <CardContent className="py-8 text-center">
            <p className="text-gray-600">This thread is locked and cannot receive new replies.</p>
          </CardContent>
        </Card>
      )}
    </div>
  );
}
