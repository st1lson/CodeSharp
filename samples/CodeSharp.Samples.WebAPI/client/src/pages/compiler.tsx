import { useState } from "react";
import { OnChange } from "@monaco-editor/react";
import { compile } from "@/services/compilation.service";
import CodeEditor from "@/components/codeEditor";

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
    const [output, setOutput] = useState<string>("");
    const [isLoading, setIsLoading] = useState<boolean>(false);

    const handleCodeChange: OnChange = (newCode: string | undefined) => {
        if (!newCode) return;

        setCode(newCode);
    };

    const handleCompile = async () => {
        try {
            setIsLoading(true);
            const response = await compile(code);
            setOutput(response.output);
        } catch (error) {
            console.error("Compilation error:", error);
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="flex flex-col p-4 text-white">
            <div className="flex-grow mb-4">
                <CodeEditor
                    code={code}
                    onChange={handleCodeChange}
                    height="60vh"
                />
            </div>
            <div className="flex flex-col gap-5 items-center">
                <div className="w-full">
                    <h3 className="text-xl font-bold mb-2">Output:</h3>
                    <pre className="bg-gray-800 p-4 w-full">{output}</pre>
                </div>
                <button
                    className={`bg-blue-500 text-white px-6 py-3 rounded mt-4 md:mt-0 ${
                        isLoading ? "opacity-50 cursor-not-allowed" : ""
                    }`}
                    onClick={handleCompile}
                    disabled={isLoading}
                >
                    {isLoading ? "Compiling..." : "Compile"}
                </button>
            </div>
        </div>
    );
};

export default Compiler;
