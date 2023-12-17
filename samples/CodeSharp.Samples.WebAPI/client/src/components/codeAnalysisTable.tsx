import { CodeReport } from "@/models/compilation";

interface Props {
    codeReport: CodeReport;
}

const CodeAnalysisTable: React.FC<Props> = ({ codeReport }) => {
    return (
        <div className="mt-4 p-4 rounded-md">
            <h3 className="text-xl font-bold mb-2">Code Analysis:</h3>
            <div className="overflow-x-auto">
                <table className="min-w-full divide-y divide-gray-200">
                    <thead className="bg-gray-700 text-white">
                        <tr>
                            <th className="px-6 py-3 text-left text-xs font-medium uppercase tracking-wider">
                                Line
                            </th>
                            <th className="px-6 py-3 text-left text-xs font-medium uppercase tracking-wider">
                                Column
                            </th>
                            <th className="px-6 py-3 text-left text-xs font-medium uppercase tracking-wider">
                                Code
                            </th>
                            <th className="px-6 py-3 text-left text-xs font-medium uppercase tracking-wider">
                                Severity
                            </th>
                            <th className="px-6 py-3 text-left text-xs font-medium uppercase tracking-wider">
                                Message
                            </th>
                        </tr>
                    </thead>
                    <tbody className="bg-gray-600 divide-y divide-gray-200">
                        {codeReport.errors.map((error, index) => (
                            <tr key={index} className="text-white">
                                <td className="px-6 py-6 whitespace-nowrap">
                                    {error.line}
                                </td>
                                <td className="px-6 py-6 whitespace-nowrap">
                                    {error.column}
                                </td>
                                <td className="px-6 py-6 whitespace-nowrap">
                                    {error.code}
                                </td>
                                <td className="px-6 py-6 whitespace-nowrap text-red-500">
                                    Error
                                </td>
                                <td className="px-6 py-6 whitespace-pre-wrap break-words">
                                    {error.message}
                                </td>
                            </tr>
                        ))}
                        {codeReport.codeAnalysisIssues.map((issue, index) => (
                            <tr key={index} className="text-white">
                                <td className="px-6 py-6 whitespace-nowrap">
                                    {issue.line}
                                </td>
                                <td className="px-6 py-6 whitespace-nowrap">
                                    {issue.column}
                                </td>
                                <td className="px-6 py-6 whitespace-nowrap">
                                    {issue.code}
                                </td>
                                <td className="px-6 py-6 whitespace-nowrap text-yellow-500">
                                    Warning
                                </td>
                                <td className="px-6 py-6 whitespace-pre-wrap break-words">
                                    {issue.message}
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default CodeAnalysisTable;
