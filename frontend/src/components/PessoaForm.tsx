import { useState } from "react";
import { apiPost } from "../services/api";
import type { Pessoa } from "../types";

interface Props {
    onPessoaCriada: (pessoa: Pessoa) => void;
}

export function PessoaForm({ onPessoaCriada }: Props) {
    const [nome, setNome] = useState("");
    const [idade, setIdade] = useState("");
    const [erro, setErro] = useState("");

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault();
        setErro("");
        try {
            const pessoa = await apiPost<Pessoa>("/Pessoas", { nome, idade: Number(idade) });
            onPessoaCriada(pessoa);
            setNome("");
            setIdade("");
        } catch (err) {
            setErro(err instanceof Error ? err.message : "Erro ao criar pessoa.");
        }
    }

    return (
        <div className="bg-[#1f2937]/50 border border-gray-700/50 rounded-2xl p-6 shadow-xl">
            <h2 className="text-xl font-semibold mb-6">Cadastrar Pessoa</h2>
            <form className="space-y-4" onSubmit={handleSubmit}>
                <input
                    className="w-full p-3 rounded-lg bg-white text-gray-900 border-none focus:ring-2 focus:ring-blue-500"
                    placeholder="Nome"
                    value={nome}
                    onChange={(e) => setNome(e.target.value)}
                    required
                />
                <input
                    className="w-full p-3 rounded-lg bg-white text-gray-900 border-none focus:ring-2 focus:ring-blue-500"
                    type="number"
                    placeholder="Idade"
                    value={idade}
                    onChange={(e) => setIdade(e.target.value)}
                    required
                />
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