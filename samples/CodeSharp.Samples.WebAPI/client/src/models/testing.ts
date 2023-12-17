export interface Test {
    id: string;
    testsCode: string;
    initialUserCode: string;
    description: string;
}

export interface TestingResponse {
    success: boolean;
    testResults: [];
}
