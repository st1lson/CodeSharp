import { CompilationRequestParams, CompilationResponse } from '@/models/compilation';
import api from './axios';

export const compile = async (params: CompilationRequestParams): Promise<CompilationResponse> => {
	const maxCompilationTimeInMilliseconds = params.maxCompilationTime * 1000;
	const maxExecutionTimeInMilliseconds = params.maxExecutionTime * 1000;

	const response = await api.post<CompilationResponse>('/api/compilation/', {
		code: params.code,
		maxCompilationTime: maxCompilationTimeInMilliseconds,
		maxRamUsage: params.maxRamUsage,
		run: params.run,
		maxExecutionTime: maxExecutionTimeInMilliseconds,
		inputs: params.inputs
	});

	return response.data;
};
