export interface CompilationResponse {
    success: boolean;
    output: string;
    duration: string;
    codeReport: CodeReport;
}

export interface CodeReport {
    errors: CodeAnalysisNode[];
    codeAnalysisIssues: CodeAnalysisNode[];
}

export interface CodeAnalysisNode {
    line: number;
    column: number;
    code: string;
    severity: string;
    message: string;
}
