import type { Pessoa, Transacao } from "../types";

interface Props {
    transacoes: Transacao[];
    pessoas: Pessoa[];
}

export function TransacaoList({ transacoes, pessoas }: Props) {
    function nomeDaPessoa(pessoaId: number) {
        return pessoas.find((p) => p.id === pessoaId)?.nome ?? "Desconhecida";
    }

    return (
        <div className="bg-[#1f2937]/50 border border-gray-700/50 rounded-2xl p-6 shadow-xl h-full min-h-[600px]">
            <h2 className="text-xl font-semibold mb-6">Últimas Transações</h2>
            <div className="space-y-4">
                {transacoes.map((t) => (
                    <div key={t.id} className="bg-white rounded-[2rem] p-4 flex justify-between items-center text-gray-900 shadow-md">
                        <div className="text-sm">
                            <p className="font-medium text-base">{t.descricao}</p>
                            <p className="text-gray-500 font-semibold">({nomeDaPessoa(t.pessoaId)})</p>
                        </div>
                        <div className={`${t.tipo === "Receita" ? "bg-green-600" : "bg-red-500"} text-white px-4 py-1 rounded-full text-sm font-bold`}>
                            R$ {t.valor.toFixed(2)}
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
}