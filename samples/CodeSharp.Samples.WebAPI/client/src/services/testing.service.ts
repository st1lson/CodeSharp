import { Test, TestingRequest, TestingResponse } from "@/models/testing";
import api from "./axios";

export const fetchTest = async (): Promise<Test> => {
    const response = await api.get<Test>("/api/testing/");

    return response.data;
};

export const startTesting = async (
    request: TestingRequest
): Promise<TestingResponse> => {
    console.log(request);
    const response = await api.post<TestingResponse>("/api/testing", request);

    return response.data;
};
