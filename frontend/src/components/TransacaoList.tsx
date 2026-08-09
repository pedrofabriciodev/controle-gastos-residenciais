import { useState } from "react";
import { buscarTransacoes, ordenarTransacoes } from "../services/api";
import type { Pessoa, Transacao } from "../types";

interface Props {
    transacoes: Transacao[];
    pessoas: Pessoa[];
}

export function TransacaoList({ transacoes, pessoas }: Props) {
    const [termo, setTermo] = useState("");
    const [listaExibida, setListaExibida] = useState<Transacao[] | null>(null);
    const [erro, setErro] = useState("");

    function nomeDaPessoa(pessoaId: number) {
        return pessoas.find((p) => p.id === pessoaId)?.nome ?? "Desconhecida";
    }

    async function handleBuscar(e: React.FormEvent) {
        e.preventDefault();
        setErro("");
        try {
            if (termo.trim() === "") {
                setListaExibida(null);
                return;
            }
            const resultado = await buscarTransacoes(termo);
            setListaExibida(resultado);
        } catch (err) {
            setErro(err instanceof Error ? err.message : "Erro ao buscar transações.");
        }
    }

    const [ordemAscendente, setOrdemAscendente] = useState(true);

    async function handleOrdenar() {
        setErro("");
        try {
            const resultado = await ordenarTransacoes(ordemAscendente);
            setListaExibida(resultado);
            setOrdemAscendente(!ordemAscendente); // inverte para o próximo clique
        } catch (err) {
            setErro(err instanceof Error ? err.message : "Erro ao ordenar transações.");
        }
    }

    function limparFiltro() {
        setTermo("");
        setListaExibida(null);
    }

    const lista = listaExibida ?? transacoes;

    return (
        <div className="bg-[var(--surface)] border border-[var(--border)] rounded-xl p-6 h-full min-h-[600px]">
            <h2 className="text-xl font-semibold mb-6">Últimas Transações</h2>

            <form onSubmit={handleBuscar} className="flex gap-2 mb-4">
                <input
                    className="flex-1 p-2 rounded-lg bg-white text-gray-900 border-none focus:ring-2 focus:ring-blue-500 text-sm"
                    placeholder="Buscar por descrição..."
                    value={termo}
                    onChange={(e) => setTermo(e.target.value)}
                />
                <button
                    type="submit"
                    className="bg-blue-600 hover:bg-blue-700 text-white px-3 py-2 rounded-lg text-sm"
                >
                    Buscar
                </button>
            </form>

            <div className="flex gap-2 mb-4">
                <button
                    onClick={handleOrdenar}
                    className="bg-gray-700 hover:bg-gray-600 text-white px-3 py-2 rounded-lg text-sm"
                >
                    Ordenar ({ordemAscendente ? "menor → maior" : "maior → menor"})
                </button>
                {listaExibida && (
                    <button
                        onClick={limparFiltro}
                        className="bg-gray-700 hover:bg-gray-600 text-white px-3 py-2 rounded-lg text-sm"
                    >
                        Limpar filtro
                    </button>
                )}
            </div>

            {erro && <p className="text-red-400 text-sm mb-4">{erro}</p>}

            <div className="space-y-4">
                {lista.map((t) => (
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