import { Money } from "./Money";

interface Props {
    saldoGeral: number | null;
}

export function Header({ saldoGeral }: Props) {
    return (
        <header className="flex justify-between items-center mb-10 max-w-7xl mx-auto px-4">
            <div className="flex items-center gap-3">
                <div className="w-9 h-9 rounded-lg bg-gradient-to-br from-indigo-500 to-indigo-700 flex items-center justify-center font-bold text-sm">
                    CG
                </div>
                <div>
                    <h1 className="text-lg font-semibold tracking-tight leading-none">Controle de Gastos</h1>
                    <p className="text-xs text-gray-500 mt-1">Gestão financeira residencial</p>
                </div>
            </div>

            {saldoGeral !== null && (
                <div className="hidden sm:flex items-center gap-2 bg-[var(--surface)] border border-[var(--border)] rounded-full px-4 py-2">
                    <span className="text-xs text-gray-400">Saldo geral</span>
                    <Money valor={saldoGeral} tone={saldoGeral >= 0 ? "success" : "danger"} />
                </div>
            )}
        </header>
    );
}