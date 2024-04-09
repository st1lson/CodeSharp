import { CompilationResponse } from '@/models/compilation';
import api from './axios';

interface CompilationRequestParams {
	code: string;
	maxCompilationTime: number;
	maxRamUsage: number;
	run: boolean;
	maxExecutionTime: number;
}

export const compile = async (params: CompilationRequestParams): Promise<CompilationResponse> => {
	const maxCompilationTimeInMilliseconds = params.maxCompilationTime * 1000;
	const maxExecutionTimeInMilliseconds = params.maxExecutionTime * 1000;

	const response = await api.post<CompilationResponse>('/api/compilation/', {
		code: params.code,
		maxCompilationTime: maxCompilationTimeInMilliseconds,
		maxRamUsage: params.maxRamUsage,
		run: params.run,
		maxExecutionTime: maxExecutionTimeInMilliseconds
	});

	return response.data;
};
