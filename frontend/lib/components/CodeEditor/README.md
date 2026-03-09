# CodeEditor Component

A fully-featured Monaco Editor integration for the ASP.NET Core Learning Platform's Browser IDE.

## Features

### ✅ C# Syntax Highlighting
- Full C# language support with Monaco Editor
- Syntax highlighting for keywords, types, and methods
- Real-time syntax error detection

### ✅ IntelliSense for .NET APIs
- Autocomplete suggestions for C# keywords (`string`, `int`, `var`, `async`, `await`)
- .NET API suggestions (`Console.WriteLine`, `Task`, `List<T>`, `Dictionary<TKey, TValue>`)
- Code snippets for common patterns (public class, public method)
- Parameter hints and documentation

### ✅ Multi-File Tab Interface
- Support for multiple files in a single project
- Tab-based navigation between files
- Add new files dynamically with the `+` button
- Remove files with the `×` button (minimum 1 file required)
- Active file highlighting

### ✅ Terminal Output Panel
- Collapsible terminal panel for execution output
- Displays stdout, stderr, and execution results
- Shows execution time and error messages
- Monospace font for code output

### ✅ Run Button
- Triggers code execution with visual feedback
- Disabled state while code is running
- Shows "Running..." status during execution

## Usage

```tsx
import { CodeEditor, CodeFile } from '@/lib/components';

const MyComponent = () => {
  const [files, setFiles] = useState<CodeFile[]>([
    {
      name: 'Program.cs',
      language: 'csharp',
      content: 'using System;\n\nclass Program {\n  static void Main() {\n    Console.WriteLine("Hello!");\n  }\n}',
    },
  ]);
  const [activeFileIndex, setActiveFileIndex] = useState(0);
  const [output, setOutput] = useState('');
  const [isRunning, setIsRunning] = useState(false);

  const handleFileChange = (index: number, content: string) => {
    const newFiles = [...files];
    newFiles[index].content = content;
    setFiles(newFiles);
  };

  const handleAddFile = (file: CodeFile) => {
    setFiles([...files, file]);
  };

  const handleRemoveFile = (index: number) => {
    if (files.length > 1) {
      setFiles(files.filter((_, i) => i !== index));
    }
  };

  const handleRun = async () => {
    setIsRunning(true);
    // Execute code...
    setIsRunning(false);
  };

  return (
    <CodeEditor
      files={files}
      activeFileIndex={activeFileIndex}
      onFileChange={handleFileChange}
      onActiveFileChange={setActiveFileIndex}
      onAddFile={handleAddFile}
      onRemoveFile={handleRemoveFile}
      onRun={handleRun}
      output={output}
      isRunning={isRunning}
    />
  );
};
```

## Props

| Prop | Type | Required | Description |
|------|------|----------|-------------|
| `files` | `CodeFile[]` | Yes | Array of code files to display |
| `activeFileIndex` | `number` | Yes | Index of the currently active file |
| `onFileChange` | `(index: number, content: string) => void` | Yes | Callback when file content changes |
| `onActiveFileChange` | `(index: number) => void` | Yes | Callback when active file changes |
| `onAddFile` | `(file: CodeFile) => void` | No | Callback to add a new file |
| `onRemoveFile` | `(index: number) => void` | No | Callback to remove a file |
| `onRun` | `() => void` | Yes | Callback when Run button is clicked |
| `output` | `string` | Yes | Terminal output to display |
| `isRunning` | `boolean` | Yes | Whether code is currently executing |

## Types

```typescript
interface CodeFile {
  name: string;        // File name (e.g., "Program.cs")
  content: string;     // File content
  language: string;    // Language identifier (e.g., "csharp")
}
```

## Editor Options

The Monaco Editor is configured with the following options:

- **Minimap**: Enabled for code overview
- **Font Size**: 14px
- **Line Numbers**: Visible
- **Tab Size**: 4 spaces
- **Word Wrap**: Enabled
- **Auto-suggestions**: Enabled
- **Parameter Hints**: Enabled
- **Format on Paste**: Enabled
- **Format on Type**: Enabled

## IntelliSense Suggestions

The component provides IntelliSense for common .NET APIs:

### Methods
- `Console.WriteLine` - Writes a line to the console
- `Console.ReadLine` - Reads a line from the console

### Types
- `string` - System.String
- `int` - System.Int32
- `Task` - System.Threading.Tasks.Task
- `Task<T>` - Generic task type
- `List<T>` - Generic list type
- `Dictionary<TKey, TValue>` - Generic dictionary type

### Keywords
- `var` - Implicitly typed variable
- `async` - Async method modifier
- `await` - Await async operation

### Snippets
- `public class` - Creates a public class template
- `public method` - Creates a public method template

## Styling

The component uses Tailwind CSS with a dark theme:

- **Background**: Gray-900 (dark)
- **Tabs**: Gray-800 with hover effects
- **Active Tab**: Gray-900 with white text
- **Terminal**: Black background with gray text
- **Run Button**: Green-600 with hover effect

## Requirements Validation

This component validates the following requirements:

- **2.1**: C# syntax highlighting ✅
- **2.2**: Autocomplete for C# keywords and .NET APIs ✅
- **2.3**: Multi-file editing support ✅
- **2.4**: Real-time syntax error display (via Monaco Editor) ✅
- **2.5**: Terminal interface for output ✅
- **2.6**: Run button for code execution ✅

## Example Page

See `/app/ide/page.tsx` for a complete example implementation with code execution integration.
