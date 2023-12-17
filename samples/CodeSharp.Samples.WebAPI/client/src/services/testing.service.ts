import { Test, TestingResponse } from "@/models/testing";
import api from "./axios";

export const fetchTest = async (): Promise<Test> => {
    const response = await api.get<Test>("/api/testing/");

    return response.data;
};

export const startTesting = async (
    testId: string,
    code: string
): Promise<TestingResponse> => {
    const response = await api.post<TestingResponse>("/api/testing", {
        testId,
        code,
    });

    return response.data;
};
