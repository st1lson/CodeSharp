import CodeEditor from "@/components/codeEditor";
import { Test } from "@/models/testing";
import { fetchTest, startTesting } from "@/services/testing.service";
import { OnChange } from "@monaco-editor/react";
import React, { useEffect, useState } from "react";

const TestPage: React.FC = () => {
    const [code, setCode] = useState<string>();
    const [test, setTest] = useState<Test | null>(null);
    const [isLoading, setIsLoading] = useState<boolean>(true);

    useEffect(() => {
        const fetchData = () => {
            fetchTest()
                .then((testData: Test) => {
                    setTest(testData);
                })
                .catch((error) => {
                    console.error("Error fetching test data:", error);
                })
                .finally(() => {
                    setIsLoading(false);
                });
        };

        fetchData();
    }, []);

    const handleCodeChange: OnChange = (newCode: string | undefined) => {
        if (!newCode) return;

        setCode(newCode);
    };

    const handleTesting = async () => {
        await startTesting(code!, test!.id);
    };

    if (isLoading) {
        return <div>Loading...</div>;
    }

    if (!test) {
        return <div>Error loading test data</div>;
    }

    return (
        <div className="flex flex-col p-4 text-white">
            <div className="flex-grow mb-4">
                <h2 className="py-2">Description: {test.description}</h2>
                <CodeEditor
                    code={test.initialUserCode}
                    onChange={handleCodeChange}
                    height="60vh"
                />
            </div>
            <div className="flex flex-col gap-5 items-center">
                <div className="w-full">
                    <h3 className="text-xl font-bold mb-2">Output:</h3>
                    <pre className="bg-gray-800 p-4 w-full">
                        {/* Display output if needed */}
                    </pre>
                </div>
                <button
                    className={`bg-blue-500 text-white px-6 py-3 rounded mt-4 md:mt-0 ${
                        isLoading ? "opacity-50 cursor-not-allowed" : ""
                    }`}
                    onClick={handleTesting}
                    disabled={isLoading}
                >
                    {isLoading ? "Testing..." : "Test"}
                </button>
            </div>
        </div>
    );
};

export default TestPage;
