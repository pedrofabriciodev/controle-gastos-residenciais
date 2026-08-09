import { Money } from "./Money";
import type { TotalGeral } from "../types";

interface Props {
    totais: TotalGeral;
}

export function StatCards({ totais }: Props) {
    const cards = [
        { label: "Receitas totais", valor: totais.totalReceitas, tone: "success" as const, accent: "border-l-emerald-400" },
        { label: "Despesas totais", valor: totais.totalDespesas, tone: "danger" as const, accent: "border-l-red-400" },
        { label: "Saldo líquido", valor: totais.saldo, tone: totais.saldo >= 0 ? "success" as const : "danger" as const, accent: totais.saldo >= 0 ? "border-l-emerald-400" : "border-l-red-400" },
    ];

    return (
        <div className="grid grid-cols-1 sm:grid-cols-3 gap-4 max-w-7xl mx-auto px-4 mb-8">
            {cards.map((c) => (
                <div
                    key={c.label}
                    className={`bg-[var(--surface)] border border-[var(--border)] border-l-4 ${c.accent} rounded-xl p-5`}
                >
                    <p className="text-xs uppercase tracking-wide text-gray-500 mb-2">{c.label}</p>
                    <p className="text-2xl">
                        <Money valor={c.valor} tone={c.tone} />
                    </p>
                </div>
            ))}
        </div>
    );
}