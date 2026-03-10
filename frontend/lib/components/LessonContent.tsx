'use client';

import { LessonDetail, Exercise } from '../types';
import { StructuredLessonView } from './StructuredLessonView';
import ReactMarkdown from 'react-markdown';
import { Prism as SyntaxHighlighter } from 'react-syntax-highlighter';
import { vscDarkPlus } from 'react-syntax-highlighter/dist/cjs/styles/prism';

export interface LessonContentProps {
  lesson: LessonDetail;
  onRunCode?: (code: string) => void;
  onStartExercise?: (exercise: Exercise) => void;
}

/**
 * Smart component that detects content type and renders appropriately.
 * Prefers structured content over HTML content for better user experience.
 * Falls back to HTML rendering if structured content is unavailable.
 * Shows a message if no content is available.
 * 
 * This component ensures backward compatibility with legacy lessons that only have HTML content.
 * 
 * @param lesson - Lesson detail object containing either structured or HTML content
 * @param onRunCode - Optional callback function when code execution is triggered
 * @param onStartExercise - Optional callback function when an exercise is started
 */
export function LessonContent({
  lesson,
  onRunCode,
  onStartExercise,
}: LessonContentProps) {
  // Debug: Log para verificar o conteúdo
  console.log('=== LESSON CONTENT DEBUG ===');
  console.log('Lesson title:', lesson?.title);
  console.log('Lesson ID:', lesson?.id);
  console.log('Has structuredContent:', !!lesson.structuredContent);
  console.log('Has content (HTML/Markdown):', !!lesson.content);
  console.log('Content length:', lesson.content?.length || 0);
  console.log('Content preview:', lesson.content?.substring(0, 200) || 'No content');
  if (lesson.structuredContent) {
    console.log('Structured content:', lesson.structuredContent);
  }
  console.log('===========================');

  // Prefer structured content over HTML content, but also check for regular content
  if (lesson.structuredContent) {
    return (
      <div className="lesson-content-wrapper" style={{ color: '#1f2937' }}>
        <StructuredLessonView
          content={lesson.structuredContent}
          onRunCode={onRunCode}
          onStartExercise={onStartExercise}
        />
      </div>
    );
  }

  // Fallback to HTML/Markdown content for lessons
  if (lesson.content) {
    console.log('✅ Rendering HTML/Markdown content, length:', lesson.content.length);
    
    // Check if content is markdown (starts with # or contains markdown syntax)
    const isMarkdown = lesson.content.trim().startsWith('#') || 
                       lesson.content.includes('##') ||
                       lesson.content.includes('```');
    
    console.log('📝 Content type detected:', isMarkdown ? 'Markdown' : 'HTML');
    
    if (isMarkdown) {
      // Render as markdown
      return (
        <div className="lesson-content-wrapper markdown-content" style={{ color: '#1f2937' }}>
          <div className="bg-gradient-to-br from-gray-50 to-blue-50 rounded-xl p-8 border border-gray-200">
            <ReactMarkdown
              components={{
                h1({ children, ...props }) {
                  return (
                    <h1 
                      style={{ 
                        color: '#111827', 
                        fontSize: '2.5rem',
                        fontWeight: 700,
                        marginBottom: '2rem',
                        paddingBottom: '0.75rem',
                        borderBottom: '3px solid #3b82f6',
                        background: 'linear-gradient(135deg, #3b82f6 0%, #8b5cf6 100%)',
                        WebkitBackgroundClip: 'text',
                        WebkitTextFillColor: 'transparent',
                        backgroundClip: 'text'
                      }} 
                      {...props}
                    >
                      {children}
                    </h1>
                  );
                },
                h2({ children, ...props }) {
                  return (
                    <h2 
                      style={{ 
                        color: '#111827', 
                        fontSize: '1.875rem',
                        fontWeight: 700,
                        marginTop: '2.5rem',
                        marginBottom: '1.25rem',
                        paddingBottom: '0.5rem',
                        borderBottom: '2px solid #e5e7eb'
                      }} 
                      {...props}
                    >
                      {children}
                    </h2>
                  );
                },
                h3({ children, ...props }) {
                  return (
                    <h3 
                      style={{ 
                        color: '#1f2937', 
                        fontSize: '1.5rem',
                        fontWeight: 600,
                        marginTop: '2rem',
                        marginBottom: '1rem'
                      }} 
                      {...props}
                    >
                      {children}
                    </h3>
                  );
                },
                p({ children, ...props }) {
                  return (
                    <p 
                      style={{ 
                        color: '#374151', 
                        marginBottom: '1.5rem',
                        lineHeight: '1.8',
                        fontSize: '1.125rem'
                      }} 
                      {...props}
                    >
                      {children}
                    </p>
                  );
                },
                ul({ children, ...props }) {
                  return (
                    <ul 
                      style={{ 
                        color: '#374151', 
                        paddingLeft: '0',
                        marginBottom: '2rem',
                        marginTop: '1rem',
                        listStyle: 'none',
                        background: 'linear-gradient(135deg, #f9fafb 0%, #f3f4f6 100%)',
                        borderRadius: '0.75rem',
                        padding: '1.5rem',
                        border: '1px solid #e5e7eb'
                      }} 
                      {...props}
                    >
                      {children}
                    </ul>
                  );
                },
                li({ children, ...props }) {
                  return (
                    <li 
                      style={{ 
                        color: '#374151', 
                        marginBottom: '1rem',
                        paddingLeft: '2rem',
                        position: 'relative',
                        lineHeight: '1.75',
                        fontSize: '1.0625rem'
                      }} 
                      {...props}
                    >
                      <span 
                        style={{
                          position: 'absolute',
                          left: '0.5rem',
                          top: '0.7em',
                          width: '10px',
                          height: '10px',
                          background: 'linear-gradient(135deg, #3b82f6 0%, #8b5cf6 100%)',
                          borderRadius: '50%',
                          display: 'inline-block',
                          boxShadow: '0 2px 4px rgba(59, 130, 246, 0.3)'
                        }}
                      />
                      <span style={{ display: 'inline-block' }}>{children}</span>
                    </li>
                  );
                },
                strong({ children, ...props }) {
                  return (
                    <strong 
                      style={{ 
                        color: '#1f2937', 
                        fontWeight: 700,
                        background: 'linear-gradient(135deg, #fef3c7 0%, #dbeafe 100%)',
                        padding: '0.1em 0.4em',
                        borderRadius: '0.25rem'
                      }} 
                      {...props}
                    >
                      {children}
                    </strong>
                  );
                },
                code({ node, inline, className, children, ...props }) {
                  const match = /language-(\w+)/.exec(className || '');
                  const language = match ? match[1] : '';
                  
                  return !inline && language ? (
                    <div style={{ margin: '2rem 0' }}>
                      <SyntaxHighlighter
                        style={vscDarkPlus}
                        language={language}
                        PreTag="div"
                        showLineNumbers
                        customStyle={{
                          borderRadius: '0.75rem',
                          padding: '1.5rem',
                          fontSize: '0.9375rem',
                        }}
                        {...props}
                      >
                        {String(children).replace(/\n$/, '')}
                      </SyntaxHighlighter>
                    </div>
                  ) : (
                    <code 
                      style={{ 
                        color: '#5b21b6', 
                        backgroundColor: '#ede9fe', 
                        padding: '0.25em 0.5em', 
                        borderRadius: '0.375rem',
                        fontSize: '0.9em',
                        fontWeight: 500,
                        fontFamily: 'ui-monospace, SFMono-Regular, Menlo, Monaco, Consolas, monospace'
                      }} 
                      {...props}
                    >
                      {children}
                    </code>
                  );
                },
              }}
            >
              {lesson.content}
            </ReactMarkdown>
          </div>
        </div>
      );
    }
    
    // Otherwise render as HTML
    return (
      <div
        className="html-lesson-content lesson-content-wrapper prose-lg max-w-none"
        dangerouslySetInnerHTML={{ __html: lesson.content }}
        style={{ color: '#1f2937' }}
      />
    );
  }

  // No content available
  return (
    <div className="no-content flex items-center justify-center py-12">
      <div className="text-center">
        <p className="text-gray-600 dark:text-gray-400 text-lg">
          Conteúdo não disponível.
        </p>
      </div>
    </div>
  );
}
