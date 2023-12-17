import { CompilationResponse } from '@/models/compilation';
import api from './axios';

export const compile = async (code: string): Promise<CompilationResponse> => {
	const response = await api.post<CompilationResponse>('/api/compilation/', {
		code,
	});

	return response.data;
};
