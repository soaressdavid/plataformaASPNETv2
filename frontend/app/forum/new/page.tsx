'use client';

import { useState, useEffect } from 'react';
import { useRouter, useSearchParams } from 'next/navigation';
import Link from 'next/link';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';
import { Label } from '@/components/ui/label';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import { ArrowLeft } from 'lucide-react';
import { useToast } from '@/hooks/use-toast';

const CATEGORIES = [
  'General Discussion',
  'C# Basics',
  'Object-Oriented Programming',
  'ASP.NET Core',
  'Databases & SQL',
  'Architecture & Design Patterns',
  'Career Advice',
  'Project Showcase',
  'Help & Support'
];

export default function NewThreadPage() {
  const router = useRouter();
  const searchParams = useSearchParams();
  const { toast } = useToast();
  
  const [title, setTitle] = useState('');
  const [content, setContent] = useState('');
  const [category, setCategory] = useState(searchParams.get('category') || '');
  const [submitting, setSubmitting] = useState(false);
  
  // Mock user ID - in production, get from auth context
  const currentUserId = 'user-id-placeholder';

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!title.trim()) {
      toast({
        title: 'Error',
        description: 'Please enter a title',
        variant: 'destructive'
      });
      return;
    }

    if (!content.trim()) {
      toast({
        title: 'Error',
        description: 'Please enter content',
        variant: 'destructive'
      });
      return;
    }

    if (!category) {
      toast({
        title: 'Error',
        description: 'Please select a category',
        variant: 'destructive'
      });
      return;
    }

    setSubmitting(true);
    try {
      const response = await fetch('http://localhost:5007/api/forum/threads', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          title,
          content,
          category,
          authorId: currentUserId
        })
      });

      if (response.ok) {
        const thread = await response.json();
        toast({
          title: 'Success',
          description: 'Thread created successfully'
        });
        router.push(`/forum/thread/${thread.id}`);
      } else {
        const error = await response.text();
        toast({
          title: 'Error',
          description: error || 'Failed to create thread',
          variant: 'destructive'
        });
      }
    } catch (error) {
      console.error('Error creating thread:', error);
      toast({
        title: 'Error',
        description: 'Failed to create thread',
        variant: 'destructive'
      });
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div className="container mx-auto px-4 py-8 max-w-3xl">
      <Button variant="ghost" asChild className="mb-4">
        <Link href="/forum">
          <ArrowLeft className="h-4 w-4 mr-2" />
          Back to Forum
        </Link>
      </Button>

      <Card>
        <CardHeader>
          <CardTitle className="text-2xl">Create New Thread</CardTitle>
          <CardDescription>
            Start a new discussion in the community forum
          </CardDescription>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleSubmit} className="space-y-6">
            <div className="space-y-2">
              <Label htmlFor="title">Title</Label>
              <Input
                id="title"
                placeholder="Enter a descriptive title for your thread"
                value={title}
                onChange={(e) => setTitle(e.target.value)}
                maxLength={300}
                required
              />
              <p className="text-xs text-gray-500">
                {title.length}/300 characters
              </p>
            </div>

            <div className="space-y-2">
              <Label htmlFor="category">Category</Label>
              <Select value={category} onValueChange={setCategory} required>
                <SelectTrigger id="category">
                  <SelectValue placeholder="Select a category" />
                </SelectTrigger>
                <SelectContent>
                  {CATEGORIES.map((cat) => (
                    <SelectItem key={cat} value={cat}>
                      {cat}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>

            <div className="space-y-2">
              <Label htmlFor="content">Content</Label>
              <Textarea
                id="content"
                placeholder="Write your question or discussion topic here... (Markdown supported)"
                value={content}
                onChange={(e) => setContent(e.target.value)}
                rows={12}
                required
              />
              <p className="text-xs text-gray-500">
                You can use markdown formatting for code blocks, lists, and more
              </p>
            </div>

            <div className="bg-blue-50 p-4 rounded-lg">
              <h3 className="font-semibold mb-2">Tips for a great thread:</h3>
              <ul className="text-sm space-y-1 text-gray-700">
                <li>• Use a clear, descriptive title</li>
                <li>• Provide context and details about your question</li>
                <li>• Include code examples when relevant (use markdown code blocks)</li>
                <li>• Be specific about what you've tried and what's not working</li>
                <li>• Search existing threads to avoid duplicates</li>
              </ul>
            </div>

            <div className="flex gap-3">
              <Button type="submit" disabled={submitting} className="flex-1">
                {submitting ? 'Creating...' : 'Create Thread'}
              </Button>
              <Button
                type="button"
                variant="outline"
                onClick={() => router.back()}
                disabled={submitting}
              >
                Cancel
              </Button>
            </div>
          </form>
        </CardContent>
      </Card>

      <Card className="mt-6">
        <CardHeader>
          <CardTitle className="text-lg">Markdown Formatting Guide</CardTitle>
        </CardHeader>
        <CardContent className="text-sm space-y-2">
          <div>
            <strong>Code blocks:</strong>
            <pre className="bg-gray-100 p-2 rounded mt-1">
              ```csharp{'\n'}
              public class Example {'{'}...{'}'}
              {'\n'}```
            </pre>
          </div>
          <div>
            <strong>Inline code:</strong> <code>`var x = 10;`</code>
          </div>
          <div>
            <strong>Bold:</strong> <code>**bold text**</code>
          </div>
          <div>
            <strong>Italic:</strong> <code>*italic text*</code>
          </div>
          <div>
            <strong>Lists:</strong> <code>- Item 1</code>, <code>- Item 2</code>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}
