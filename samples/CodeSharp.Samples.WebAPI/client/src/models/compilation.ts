export interface CompilationRequestParams {
    code: string;
    maxCompilationTime: number;
    maxRamUsage: number;
    run: boolean;
    maxExecutionTime: number;
    inputs: string[];
}

export interface CompilationResponse {
    compiledSuccessfully: boolean;
    executedSuccessfully: boolean;
    compilationDuration: string;
    executionDuration: string;
    output: string;
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
