import CodeAnalysisTable from '@/components/codeAnalysisTable';
import CodeEditor from '@/components/codeEditor';
import { Test, TestingRequest, TestingResponse } from '@/models/testing';
import { fetchTest, startTesting } from '@/services/testing.service';
import { OnChange } from '@monaco-editor/react';
import React, { useEffect, useState } from 'react';

const TestPage: React.FC = () => {
	const [code, setCode] = useState<string>();
	const [test, setTest] = useState<Test | null>(null);
	const [testResult, setTestResult] = useState<TestingResponse | null>(null);
	const [isLoading, setIsLoading] = useState<boolean>(true);

	useEffect(() => {
		const fetchData = async () => {
			try {
				const testData = await fetchTest();

				setCode(testData.initialUserCode);
				setTest(testData);
			} catch (error) {
				console.error('Error fetching test data:', error);
			} finally {
				setIsLoading(false);
			}
		};

		fetchData();
	}, []);

	const handleCodeChange: OnChange = (newCode: string | undefined) => {
		if (!newCode) return;

		setCode(newCode);
	};

	const handleTesting = async () => {
		try {
			setIsLoading(true);
			setTestResult(
				await startTesting({
					code: code!,
					testId: test!.id,
				} as TestingRequest)
			);
		} finally {
			setIsLoading(false);
		}
	};

	if (!test) {
		return <div>Error loading test data</div>;
	}

	return (
		<div className='flex flex-col p-4 text-white'>
			<div className='flex-grow mb-4'>
				<h2 className='py-2'>Description: {test.description}</h2>
				<CodeEditor
					code={test.initialUserCode}
					onChange={handleCodeChange}
					height='60vh'
				/>
			</div>
			<div className='flex flex-col gap-5 items-center'>
				<div className='w-full'>
					{testResult ? (
						<pre className='bg-gray-800 p-4 w-full'>
							{testResult?.testResults && testResult.testResults.length > 0 && (
								<div className='flex flex-col gap-5'>
									<h3 className='text-xl mt-4 font-bold mb-2'>Test results:</h3>
									{testResult.testResults.map((result, index) => (
										<div
											key={index}
											className={`p-4 rounded-md ${
												result.passed ? 'bg-green-700' : 'bg-red-700'
											}`}
										>
											<p
												className={`font-bold text-${
													result.passed ? 'green' : 'red'
												}-100`}
											>
												Test: {result.testName}
											</p>
											<p
												className={`text-${
													result.passed ? 'green' : 'red'
												}-100`}
											>
												Passed: {result.passed ? 'Yes' : 'No'}
											</p>
											<p
												className={`text-${
													result.passed ? 'green' : 'red'
												}-100`}
											>
												Execution Time: {result.executionTime} ms
											</p>
											{result.errorMessage && (
												<p className='text-red-100'>
													Error: {result.errorMessage}
												</p>
											)}
										</div>
									))}
								</div>
							)}
							{testResult && testResult.codeReport && (
								<CodeAnalysisTable codeReport={testResult.codeReport} />
							)}
						</pre>
					) : null}
				</div>
				<button
					className={`bg-blue-500 text-white px-6 py-3 rounded mt-4 md:mt-0 ${
						isLoading ? 'opacity-50 cursor-not-allowed' : ''
					}`}
					onClick={handleTesting}
					disabled={isLoading}
				>
					{isLoading ? 'Testing...' : 'Test'}
				</button>
			</div>
		</div>
	);
};

export default TestPage;
