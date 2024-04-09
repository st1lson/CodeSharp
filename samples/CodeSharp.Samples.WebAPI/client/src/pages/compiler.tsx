import { useState } from "react";
import { OnChange } from "@monaco-editor/react";
import { compile } from "@/services/compilation.service";
import CodeEditor from "@/components/codeEditor";
import { CompilationResponse } from "@/models/compilation";
import CodeAnalysisTable from "@/components/codeAnalysisTable";

const Compiler: React.FC = () => {
    const [code, setCode] = useState<string>(
        `
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace CodeSharp.Runner
    {
      class Program
      {
        static void Main(string[] args)
        {
			Console.WriteLine("Hello World!");
        }
      }
    }
  `
            .trim()
            .replace(/^ {4}/gm, "")
    );
    const [compilationResult, setCompilationResult] =
        useState<CompilationResponse>();
    const [isLoading, setIsLoading] = useState<boolean>(false);

    const [run, setRun] = useState<boolean>(true);
    const [maxCompilationTime, setMaxCompilationTime] = useState<string>('10');
    const [maxRamUsage, setMaxRamUsage] = useState<string>('10');
    const [maxExecutionTime, setMaxExecutionTime] = useState<string>('15');
    const [inputs, setInputs] = useState<string>('');

    const handleCodeChange: OnChange = (newCode: string | undefined) => {
        if (!newCode) return;

        setCode(newCode);
    };

    const handleCompile = async () => {
        try {
            setIsLoading(true);
            const compilationRequest = {
                code,
                maxCompilationTime: parseInt(maxCompilationTime, 10),
                maxRamUsage: parseInt(maxRamUsage, 10),
                maxExecutionTime: parseInt(maxExecutionTime, 10),
                inputs: inputs.split(',').filter(input => input.trim()), // Split inputs by comma and remove empty values
                run
            };
            const response = await compile(compilationRequest);
            setCompilationResult(response);
        } catch (error) {
            console.error("Compilation error:", error);
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="flex flex-col p-4 text-white">
            <div className="flex-grow mb-4">
                <CodeEditor code={code} onChange={handleCodeChange} height="60vh" />
            </div>
            <div className="grid grid-cols-2 gap-4">
                <div className="flex flex-col">
                    <label htmlFor="maxCompilationTime" className="text-white mb-1">Max Compilation Time (seconds)</label>
                    <input
                        type="text"
                        id="maxCompilationTime"
                        value={maxCompilationTime}
                        onChange={(e) => setMaxCompilationTime(e.target.value)}
                        placeholder="Max Compilation Time"
                        className="input rounded text-black"
                    />
                </div>
                <div className="flex flex-col">
                    <label htmlFor="maxRamUsage" className="text-white mb-1">Max RAM Usage (MB)</label>
                    <input
                        type="text"
                        id="maxRamUsage"
                        value={maxRamUsage}
                        onChange={(e) => setMaxRamUsage(e.target.value)}
                        placeholder="Max RAM Usage"
                        className="input rounded text-black"
                    />
                </div>
                <div className="flex flex-col">
                    <label htmlFor="maxExecutionTime" className="text-white mb-1">Max Execution Time (seconds)</label>
                    <input
                        type="text"
                        id="maxExecutionTime"
                        value={maxExecutionTime}
                        onChange={(e) => setMaxExecutionTime(e.target.value)}
                        placeholder="Max Execution Time"
                        className="input rounded text-black"
                    />
                </div>
                <div className="flex flex-col">
                    <label htmlFor="inputs" className="text-white mb-1">Inputs (comma-separated)</label>
                    <input
                        type="text"
                        id="inputs"
                        value={inputs}
                        onChange={(e) => setInputs(e.target.value)}
                        placeholder="Inputs"
                        className="input rounded text-black"
                    />
                </div>
                <div className="flex items-center">
                    <input
                        type="checkbox"
                        id="run"
                        checked={run}
                        onChange={(e) => setRun(e.target.checked)}
                        className="form-checkbox h-5 w-5 text-blue-600"
                    />
                    <label htmlFor="run" className="text-white ml-2">Run</label>
                </div>
            </div>
            <div className="flex flex-col gap-5 items-center mb-4">
                ...
                <button
                    className={`bg-blue-500 text-white px-6 py-3 rounded mt-4 ${
                        isLoading ? "opacity-50 cursor-not-allowed" : ""
                    }`}
                    onClick={handleCompile}
                    disabled={isLoading}
                >
                    {isLoading ? "Compiling..." : "Compile"}
                </button>
            </div>
            <div className='flex flex-col gap-5 items-center'>
				<div className='w-full'>
                    {compilationResult ? (
						<pre className='bg-gray-800 p-4 w-full'>
                            <h3 className='text-xl mt-4 font-bold mb-2'>Compilation info:</h3>
                            <p>{compilationResult.compiledSuccessfully ? 'Compiled successfully' : 'Compilation failed'} </p>
                            <p>Compilation time: {compilationResult.compilationDuration}</p>
                            {compilationResult.executedSuccessfully ? <p>{compilationResult.executedSuccessfully ? 'Executed successfully' : 'Execution failed'} </p> : null}
                            {compilationResult.executionDuration ? <p>Execution time: {compilationResult.executionDuration}</p> : null}
                            {compilationResult.output ? <p>Output: {compilationResult.output}</p> : null}
                            {compilationResult && compilationResult.codeReport && (
                                <CodeAnalysisTable codeReport={compilationResult.codeReport} />
                            )}
                        </pre>)
                    : null }
                </div>
            </div>
        </div>
    );
};

export default Compiler;
