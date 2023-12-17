import { CodeReport } from "./compilation";

export interface Test {
    id: string;
    testsCode: string;
    initialUserCode: string;
    description: string;
}

export interface TestingRequest {
    testId: string;
    code: string;
}

export interface TestingResponse {
    success: boolean;
    testResults: TestReport[];
    codeReport: CodeReport;
}

export interface TestReport {
    testName: string;
    passed: boolean;
    executionTime: number;
    errorMessage?: string;
}
