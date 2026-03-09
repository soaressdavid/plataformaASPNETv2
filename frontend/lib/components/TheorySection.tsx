import ReactMarkdown from 'react-markdown';
import { Prism as SyntaxHighlighter } from 'react-syntax-highlighter';
import { vscDarkPlus } from 'react-syntax-highlighter/dist/cjs/styles/prism';

export interface TheorySectionProps {
  heading: string;
  content: string; // markdown
  order: number;
}

/**
 * Component to render theory sections with markdown support.
 * Supports code blocks with syntax highlighting.
 * 
 * @param heading - Section heading/title
 * @param content - Markdown content to render
 * @param order - Display order (used for sorting)
 */
export function TheorySection({ heading, content }: TheorySectionProps) {
  if (!content) {
    return null;
  }

  return (
    <section className="theory-section mb-8 bg-gradient-to-br from-gray-50 to-blue-50 rounded-xl p-6 border border-gray-200">
      <h3 className="text-2xl font-bold mb-4 bg-gradient-to-r from-blue-600 to-purple-600 bg-clip-text text-transparent">
        {heading}
      </h3>
      <div className="theory-content max-w-none prose-custom">
        <ReactMarkdown
          components={{
            code({ node, inline, className, children, ...props }) {
              const match = /language-(\w+)/.exec(className || '');
              const language = match ? match[1] : '';
              
              return !inline && language ? (
                <div className="my-4">
                  <SyntaxHighlighter
                    style={vscDarkPlus}
                    language={language}
                    PreTag="div"
                    showLineNumbers
                    customStyle={{
                      borderRadius: '0.75rem',
                      padding: '1.5rem',
                      fontSize: '0.9rem',
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
            p({ children, ...props }) {
              return (
                <p 
                  style={{ 
                    color: '#374151', 
                    marginBottom: '1.25rem',
                    lineHeight: '1.8',
                    fontSize: '1.05rem'
                  }} 
                  {...props}
                >
                  {children}
                </p>
              );
            },
            strong({ children, ...props }) {
              return (
                <strong 
                  style={{ 
                    color: '#1f2937', 
                    fontWeight: 700,
                    background: 'linear-gradient(135deg, #fef3c7 0%, #dbeafe 100%)',
                    padding: '0.1em 0.3em',
                    borderRadius: '0.25rem'
                  }} 
                  {...props}
                >
                  {children}
                </strong>
              );
            },
            ul({ children, ...props }) {
              return (
                <ul 
                  style={{ 
                    color: '#374151', 
                    paddingLeft: '0',
                    marginBottom: '1.75rem',
                    marginTop: '1rem',
                    listStyle: 'none',
                    background: 'linear-gradient(135deg, #f9fafb 0%, #f3f4f6 100%)',
                    borderRadius: '0.75rem',
                    padding: '1.25rem',
                    border: '1px solid #e5e7eb'
                  }} 
                  {...props}
                >
                  {children}
                </ul>
              );
            },
            ol({ children, ...props }) {
              return (
                <ol 
                  style={{ 
                    color: '#374151', 
                    paddingLeft: '2rem',
                    marginBottom: '1.75rem',
                    marginTop: '1rem',
                    listStyle: 'decimal',
                    background: 'linear-gradient(135deg, #f9fafb 0%, #f3f4f6 100%)',
                    borderRadius: '0.75rem',
                    padding: '1.25rem 1.25rem 1.25rem 3rem',
                    border: '1px solid #e5e7eb',
                    counterReset: 'list-counter'
                  }} 
                  {...props}
                >
                  {children}
                </ol>
              );
            },
            li({ children, ...props }) {
              const parent = props.node?.parent;
              const isOrdered = parent && parent.type === 'element' && parent.tagName === 'ol';
              
              return (
                <li 
                  style={{ 
                    color: '#374151', 
                    marginBottom: '0.875rem',
                    paddingLeft: isOrdered ? '0' : '2rem',
                    position: 'relative',
                    lineHeight: '1.75',
                    fontSize: '1.05rem'
                  }} 
                  {...props}
                >
                  {!isOrdered && (
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
                  )}
                  <span style={{ display: 'inline-block' }}>{children}</span>
                </li>
              );
            },
            h1({ children, ...props }) {
              return (
                <h1 
                  style={{ 
                    color: '#111827', 
                    fontSize: '2rem',
                    fontWeight: 700,
                    marginTop: '2rem',
                    marginBottom: '1rem',
                    paddingBottom: '0.5rem',
                    borderBottom: '3px solid #e5e7eb'
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
                    fontSize: '1.5rem',
                    fontWeight: 700,
                    marginTop: '1.75rem',
                    marginBottom: '0.875rem'
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
                    fontSize: '1.25rem',
                    fontWeight: 600,
                    marginTop: '1.5rem',
                    marginBottom: '0.75rem'
                  }} 
                  {...props}
                >
                  {children}
                </h3>
              );
            },
          }}
        >
          {content}
        </ReactMarkdown>
      </div>
    </section>
  );
}
