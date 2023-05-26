interface LayoutProps {
  children?: React.ReactNode;
}

export default function Layout({ children }: LayoutProps) {
  return (
    <div
      style={{ backgroundColor: 'black' }}
      className="mx-auto flex flex-col space-y-4"
    >
      <header className="container sticky top-0 z-40 bg-white">
        <div
          style={{ backgroundColor: '#1c1e22' }}
          className="h-16 border-b border-b-slate-200 py-4"
        >
          <nav className="ml-4 pl-6">
            <a
              href="#"
              style={{ color: '#c8c8c8' }}
              className="hover:text-slate-600 cursor-pointer"
            >
              Home
            </a>
          </nav>
        </div>
      </header>
      <div>
        <main
          style={{ backgroundColor: 'black' }}
          className="flex w-full flex-1 flex-col overflow-hidden"
        >
          {children}
        </main>
      </div>
    </div>
  );
}
