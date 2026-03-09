# AI Feedback Component

The AI Feedback component displays AI-powered code review feedback with categorized suggestions.

## Features

- **Categorized Feedback**: Suggestions grouped by type (Security, Performance, Best Practice, Architecture)
- **Overall Score**: Visual score indicator with color-coded rating
- **Line Numbers**: Each suggestion includes the line number for easy navigation
- **Code Examples**: Suggested code improvements with syntax highlighting
- **Issue Summary**: Quick overview of security and performance issues
- **Loading State**: Animated loading indicator while AI analyzes code

## Usage

```tsx
import { AIFeedback } from '@/lib/components';
import { useAIFeedback } from '@/lib/hooks/useAIFeedback';

function MyComponent() {
  const { feedback, isLoading, getFeedback, clearFeedback } = useAIFeedback();
  const [showFeedback, setShowFeedback] = useState(false);

  const handleGetFeedback = async () => {
    setShowFeedback(true);
    await getFeedback(code, context);
  };

  const handleClose = () => {
    setShowFeedback(false);
    clearFeedback();
  };

  return (
    <AIFeedback
      suggestions={feedback?.suggestions || []}
      overallScore={feedback?.overallScore || 0}
      securityIssues={feedback?.securityIssues || []}
      performanceIssues={feedback?.performanceIssues || []}
      isLoading={isLoading}
      onClose={handleClose}
    />
  );
}
```

## Props

| Prop | Type | Description |
|------|------|-------------|
| `suggestions` | `Feedback[]` | Array of feedback suggestions |
| `overallScore` | `number` | Overall code quality score (0-100) |
| `securityIssues` | `string[]` | List of security issues found |
| `performanceIssues` | `string[]` | List of performance issues found |
| `isLoading` | `boolean` | Whether AI is analyzing code |
| `onClose` | `() => void` | Callback when user closes the panel |

## Feedback Types

- **Security** 🔒: Security vulnerabilities and concerns
- **Performance** ⚡: Performance optimization suggestions
- **Best Practice** ✨: Code quality and best practice recommendations
- **Architecture** 🏗️: Architectural and design pattern suggestions

## Integration with CodeEditor

The AI Feedback component is designed to work seamlessly with the CodeEditor component:

```tsx
<CodeEditor
  // ... other props
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
```

## Styling

The component uses Tailwind CSS with color-coded categories:
- Security: Red theme
- Performance: Yellow theme
- Best Practice: Blue theme
- Architecture: Purple theme

## Requirements Validated

This component validates the following requirements:
- **4.1**: AI analyzes code using Groq API
- **4.2**: Evaluates SOLID principles compliance
- **4.3**: Evaluates clean architecture patterns
- **4.4**: Identifies security vulnerabilities
- **4.5**: Identifies performance issues
- **4.6**: Returns feedback within 10 seconds
- **4.7**: Provides specific code examples
