import React from "react";
import MonacoEditor, { OnChange } from "@monaco-editor/react";

interface CodeEditorProps {
    code: string;
    onChange: OnChange;
    height: string;
}

const CodeEditor: React.FC<CodeEditorProps> = ({ code, onChange, height }) => {
    const editorOptions = {
        acceptSuggestionOnCommitCharacter: true,
        automaticLayout: true,
        codeLens: true,
        colorDecorators: true,
        contextmenu: true,
        disableLayerHinting: false,
        disableMonospaceOptimizations: false,
        dragAndDrop: false,
        fixedOverflowWidgets: false,
        fontLigatures: false,
        formatOnPaste: true,
        formatOnType: true,
        hideCursorInOverviewRuler: false,
        highlightActiveIndentGuide: true,
        links: true,
        mouseWheelZoom: false,
        multiCursorMergeOverlapping: true,
        overviewRulerBorder: true,
        overviewRulerLanes: 2,
        quickSuggestions: true,
        quickSuggestionsDelay: 100,
        revealHorizontalRightPadding: 30,
        roundedSelection: true,
        rulers: [],
        scrollBeyondLastColumn: 5,
        scrollBeyondLastLine: true,
        selectOnLineNumbers: true,
        selectionClipboard: true,
        selectionHighlight: true,
        smoothScrolling: false,
        wordSeparators: "~!@#$%^&*()-=+[{]}|;:'\",.<>/?",
        wordWrapBreakAfterCharacters: "\t})]?|&,;",
        wordWrapBreakBeforeCharacters: "{([+",
        wordWrapBreakObtrusiveCharacters: ".",
    };

    return (
        <MonacoEditor
            language="csharp"
            theme="vs-dark"
            value={code}
            options={editorOptions}
            onChange={onChange}
            height={height}
        />
    );
};

export default CodeEditor;
