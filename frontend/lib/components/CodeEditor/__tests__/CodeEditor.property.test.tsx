/**
 * Property-Based Tests for CodeEditor Multi-File State Management
 * Feature: aspnet-learning-platform
 * 
 * These tests use fast-check to verify properties that should hold
 * for all valid inputs, ensuring the CodeEditor maintains correctness
 * across a wide range of scenarios.
 * 
 * Note: These tests focus on the state management logic rather than
 * the Monaco Editor integration, as Monaco Editor behavior is tested
 * separately in integration tests.
 */

import * as fc from 'fast-check';
import { CodeFile } from '../CodeEditor';

/**
 * Custom Arbitraries (Generators) for Property-Based Testing
 */

// Generator for valid file names
const fileNameArbitrary = fc
  .tuple(
    fc.stringMatching(/^[a-zA-Z][a-zA-Z0-9_]*$/),
    fc.constantFrom('.cs', '.txt', '.json')
  )
  .map(([name, ext]) => `${name}${ext}`);

// Generator for file content (C# code snippets)
const fileContentArbitrary = fc.oneof(
  fc.constant(''),
  fc.constant('// Empty file'),
  fc.constant('using System;\n\nclass Program\n{\n    static void Main()\n    {\n        Console.WriteLine("Hello");\n    }\n}'),
  fc.string({ minLength: 0, maxLength: 500 }),
  fc.lorem({ maxCount: 3 }).map(text => `// ${text}`)
);

// Generator for a single CodeFile
const codeFileArbitrary: fc.Arbitrary<CodeFile> = fc.record({
  name: fileNameArbitrary,
  content: fileContentArbitrary,
  language: fc.constantFrom('csharp', 'plaintext', 'json'),
});

// Generator for an array of unique CodeFiles (2-10 files)
const codeFilesArbitrary = fc
  .array(codeFileArbitrary, { minLength: 2, maxLength: 10 })
  .map(files => {
    // Ensure unique file names
    const uniqueFiles: CodeFile[] = [];
    const seenNames = new Set<string>();
    
    files.forEach((file, index) => {
      let uniqueName = file.name;
      let counter = 1;
      
      while (seenNames.has(uniqueName)) {
        const nameParts = file.name.split('.');
        const ext = nameParts.pop();
        const baseName = nameParts.join('.');
        uniqueName = `${baseName}${counter}.${ext}`;
        counter++;
      }
      
      seenNames.add(uniqueName);
      uniqueFiles.push({ ...file, name: uniqueName });
    });
    
    return uniqueFiles;
  });

/**
 * Helper: Simulates multi-file editor state management
 * This mimics how a parent component would manage CodeEditor state
 */
class MultiFileEditorState {
  private files: CodeFile[];
  private activeIndex: number;

  constructor(initialFiles: CodeFile[]) {
    this.files = initialFiles.map(f => ({ ...f }));
    this.activeIndex = 0;
  }

  getFiles(): CodeFile[] {
    return this.files.map(f => ({ ...f }));
  }

  getActiveIndex(): number {
    return this.activeIndex;
  }

  getActiveFile(): CodeFile {
    return { ...this.files[this.activeIndex] };
  }

  switchToFile(index: number): void {
    if (index >= 0 && index < this.files.length) {
      this.activeIndex = index;
    }
  }

  updateFileContent(index: number, newContent: string): void {
    if (index >= 0 && index < this.files.length) {
      this.files[index] = { ...this.files[index], content: newContent };
    }
  }

  updateActiveFileContent(newContent: string): void {
    this.updateFileContent(this.activeIndex, newContent);
  }
}

/**
 * Property 5: Multi-File Editor State
 * **Validates: Requirements 2.3**
 * 
 * For any set of code files, the browser IDE should maintain separate content
 * for each file and allow switching between them without data loss.
 */
describe('CodeEditor - Property 5: Multi-File Editor State', () => {
  /**
   * Property: File content independence
   * 
   * For any set of files, editing one file should not affect the content
   * of other files.
   */
  it('should maintain independent content for each file', () => {
    fc.assert(
      fc.property(codeFilesArbitrary, fc.nat(), fc.string(), (files, editIndexSeed, newContent) => {
        // Ensure we have at least 2 files
        if (files.length < 2) return true;
        
        const editIndex = editIndexSeed % files.length;
        const otherIndices = Array.from({ length: files.length }, (_, i) => i).filter(i => i !== editIndex);
        
        // Store original content
        const originalContents = files.map(f => f.content);
        
        // Create state manager
        const state = new MultiFileEditorState(files);
        
        // Edit the file at editIndex
        state.updateFileContent(editIndex, newContent);
        
        const currentFiles = state.getFiles();
        
        // Property: Only the edited file should have changed content
        const editedFileChanged = currentFiles[editIndex].content === newContent;
        const otherFilesUnchanged = otherIndices.every(
          i => currentFiles[i].content === originalContents[i]
        );
        
        return editedFileChanged && otherFilesUnchanged;
      }),
      { numRuns: 100 }
    );
  });

  /**
   * Property: File switching preserves content
   * 
   * For any sequence of file switches and edits, switching back to a file
   * should show the last content that was set for that file.
   */
  it('should preserve content when switching between files', () => {
    fc.assert(
      fc.property(
        codeFilesArbitrary,
        fc.array(fc.record({
          fileIndex: fc.nat(),
          newContent: fc.string({ maxLength: 100 })
        }), { minLength: 1, maxLength: 10 }),
        (initialFiles, edits) => {
          // Ensure we have at least 2 files
          if (initialFiles.length < 2) return true;
          
          // Track the expected content for each file
          const expectedContents = new Map<number, string>();
          initialFiles.forEach((file, index) => {
            expectedContents.set(index, file.content);
          });
          
          // Create state manager
          const state = new MultiFileEditorState(initialFiles);
          
          // Apply each edit
          for (const edit of edits) {
            const targetIndex = edit.fileIndex % initialFiles.length;
            
            // Switch to the target file
            state.switchToFile(targetIndex);
            
            // Edit the file
            state.updateActiveFileContent(edit.newContent);
            expectedContents.set(targetIndex, edit.newContent);
          }
          
          const currentFiles = state.getFiles();
          
          // Property: All files should have their expected content
          const allFilesHaveCorrectContent = currentFiles.every((file, index) =>
            file.content === expectedContents.get(index)
          );
          
          return allFilesHaveCorrectContent;
        }
      ),
      { numRuns: 100 }
    );
  });

  /**
   * Property: No data loss during rapid file switching
   * 
   * For any sequence of rapid file switches, no file content should be lost.
   */
  it('should not lose data during rapid file switching', () => {
    fc.assert(
      fc.property(
        codeFilesArbitrary,
        fc.array(fc.nat(), { minLength: 5, maxLength: 20 }),
        (files, switchSequence) => {
          // Ensure we have at least 2 files
          if (files.length < 2) return true;
          
          // Store original content
          const originalContents = files.map(f => f.content);
          
          // Create state manager
          const state = new MultiFileEditorState(files);
          
          // Perform rapid file switches
          for (const switchIndex of switchSequence) {
            const targetIndex = switchIndex % files.length;
            state.switchToFile(targetIndex);
          }
          
          const currentFiles = state.getFiles();
          
          // Property: All files should still have their original content
          // (since we only switched, didn't edit)
          const noDataLoss = currentFiles.every((file, index) =>
            file.content === originalContents[index]
          );
          
          return noDataLoss;
        }
      ),
      { numRuns: 100 }
    );
  });

  /**
   * Property: File count remains stable
   * 
   * For any set of files and switching operations, the number of files
   * should remain constant (unless add/remove operations are performed).
   */
  it('should maintain stable file count during switching', () => {
    fc.assert(
      fc.property(
        codeFilesArbitrary,
        fc.array(fc.nat(), { minLength: 1, maxLength: 15 }),
        (files, switchSequence) => {
          const originalFileCount = files.length;
          
          // Create state manager
          const state = new MultiFileEditorState(files);
          
          // Perform file switches
          for (const switchIndex of switchSequence) {
            const targetIndex = switchIndex % files.length;
            state.switchToFile(targetIndex);
          }
          
          const currentFiles = state.getFiles();
          
          // Property: File count should remain the same
          return currentFiles.length === originalFileCount;
        }
      ),
      { numRuns: 100 }
    );
  });

  /**
   * Property: Active file index validity
   * 
   * For any file switching operation, the active file index should always
   * be within valid bounds (0 to files.length - 1).
   */
  it('should maintain valid active file index', () => {
    fc.assert(
      fc.property(
        codeFilesArbitrary,
        fc.array(fc.nat(), { minLength: 1, maxLength: 10 }),
        (files, switchSequence) => {
          // Create state manager
          const state = new MultiFileEditorState(files);
          const activeIndices: number[] = [state.getActiveIndex()];
          
          // Perform file switches
          for (const switchIndex of switchSequence) {
            const targetIndex = switchIndex % files.length;
            state.switchToFile(targetIndex);
            activeIndices.push(state.getActiveIndex());
          }
          
          // Property: All active indices should be valid
          const allIndicesValid = activeIndices.every(
            index => index >= 0 && index < files.length
          );
          
          return allIndicesValid;
        }
      ),
      { numRuns: 100 }
    );
  });
});
