import React from 'react';
import Link from 'next/link';

interface Props {
	children: React.ReactNode;
}

const Layout: React.FC<Props> = ({ children }) => {
	return (
		<div className='min-h-screen flex flex-col p-4 bg-gray-900 text-white'>
			<nav className='fixed top-0 left-0 right-0 z-10 p-4 bg-gray-800 flex justify-between items-center'>
				<div>
					<Link href='/' className='text-2xl font-bold'>
						CodeSharp Playground
					</Link>
				</div>
				<div className='flex items-center gap-5'>
					<Link href='/compiler'>Compiler</Link>
					<Link href='/testing'>Testing</Link>
				</div>
			</nav>

			<div className='mt-16'>{children}</div>
		</div>
	);
};

export default Layout;
