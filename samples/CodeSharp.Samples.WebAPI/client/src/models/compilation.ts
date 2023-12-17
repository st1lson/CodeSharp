export interface CompilationResponse {
	success: boolean;
	output: string;
	duration: string;
	codeReport: CodeReport;
}

export interface CodeReport {
	codeAnalysis: CodeAnalysis;
}

export interface CodeAnalysis {
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
