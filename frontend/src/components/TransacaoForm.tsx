import { useState } from "react";
import { apiPost } from "../services/api";
import type { Pessoa, Transacao, TipoTransacao } from "../types";

interface Props {
    pessoas: Pessoa[];
    onTransacaoCriada: (transacao: Transacao) => void;
}

export function TransacaoForm({ pessoas, onTransacaoCriada }: Props) {
    const [descricao, setDescricao] = useState("");
    const [valor, setValor] = useState("");
    const [tipo, setTipo] = useState<TipoTransacao>("Despesa");
    const [pessoaId, setPessoaId] = useState("");
    const [erro, setErro] = useState("");

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault();
        setErro("");
        try {
            const transacao = await apiPost<Transacao>("/Transacoes", {
                descricao,
                valor: Number(valor),
                tipo,
                pessoaId: Number(pessoaId),
            });
            onTransacaoCriada(transacao);
            setDescricao("");
            setValor("");
        } catch (err) {
            setErro(err instanceof Error ? err.message : "Erro ao criar transação.");
        }
    }

    const inputClass =
        "w-full p-3 rounded-lg bg-white text-gray-900 border-none focus:ring-2 focus:ring-blue-500";

    return (
        <div className="bg-[var(--surface)] border border-[var(--border)] rounded-xl p-6">
            <h2 className="text-xl font-semibold mb-6">Cadastrar Transação</h2>
            <form className="space-y-4" onSubmit={handleSubmit}>
                <select
                    className={`${inputClass} text-gray-500`}
                    value={pessoaId}
                    onChange={(e) => setPessoaId(e.target.value)}
                    required
                >
                    <option value="" disabled>Selecione a pessoa</option>
                    {pessoas.map((p) => (
                        <option key={p.id} value={p.id}>{p.nome}</option>
                    ))}
                </select>

                <input
                    className={inputClass}
                    placeholder="Descrição"
                    value={descricao}
                    onChange={(e) => setDescricao(e.target.value)}
                    required
                />

                <input
                    className={inputClass}
                    type="number"
                    step="0.01"
                    placeholder="Valor"
                    value={valor}
                    onChange={(e) => setValor(e.target.value)}
                    required
                />

                <select
                    className={`${inputClass} text-gray-500`}
                    value={tipo}
                    onChange={(e) => setTipo(e.target.value as TipoTransacao)}
                >
                    <option value="Despesa">Despesa</option>
                    <option value="Receita">Receita</option>
                </select>

                <button
                    className="w-full bg-blue-600 hover:bg-blue-700 text-white font-medium py-3 rounded-lg transition-colors duration-200"
                    type="submit"
                >
                    Cadastrar
                </button>
                {erro && <p className="text-red-400 text-sm">{erro}</p>}
            </form>
        </div>
    );
}