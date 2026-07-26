import type { TotalGeral } from "../types";

interface Props {
    totais: TotalGeral;
}

export function ResumoFinanceiro({ totais }: Props) {
    return (
        <div className="bg-black/40 border border-gray-700/50 rounded-2xl p-6 shadow-xl">
            <h2 className="text-xl font-semibold mb-6">Resumo Financeiro</h2>
            <div className="overflow-x-auto">
                <table className="w-full text-left">
                    <thead>
                    <tr className="text-sm font-medium border-b border-gray-700">
                        <th className="pb-4">Pessoa</th>
                        <th className="pb-4">Receitas</th>
                        <th className="pb-4">Despesas</th>
                        <th className="pb-4">Saldo</th>
                    </tr>
                    </thead>
                    <tbody className="text-sm">
                    {totais.porPessoa.map((p) => (
                        <tr key={p.pessoaId} className="border-b border-gray-700/50">
                            <td className="py-4">{p.nome}</td>
                            <td className="py-4 text-green-400">R$ {p.totalReceitas.toFixed(2)}</td>
                            <td className="py-4 text-red-400">R$ {p.totalDespesas.toFixed(2)}</td>
                            <td className={`py-4 ${p.saldo >= 0 ? "text-green-400" : "text-red-400"}`}>
                                R$ {p.saldo.toFixed(2)}
                            </td>
                        </tr>
                    ))}
                    <tr className="font-bold">
                        <td className="py-4">Total Geral</td>
                        <td className="py-4 text-green-400">R$ {totais.totalReceitas.toFixed(2)}</td>
                        <td className="py-4 text-red-400">R$ {totais.totalDespesas.toFixed(2)}</td>
                        <td className={`py-4 ${totais.saldo >= 0 ? "text-green-400" : "text-red-400"}`}>
                            R$ {totais.saldo.toFixed(2)}
                        </td>
                    </tr>
                    </tbody>
                </table>
            </div>
        </div>
    );
}