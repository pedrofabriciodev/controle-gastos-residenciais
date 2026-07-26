import { apiDelete } from "../services/api";
import type { Pessoa } from "../types";

interface Props {
    pessoas: Pessoa[];
    onPessoaRemovida: (id: number) => void;
}

export function PessoaList({ pessoas, onPessoaRemovida }: Props) {
    async function handleDeletar(id: number) {
        try {
            await apiDelete(`/Pessoas/${id}`);
            onPessoaRemovida(id);
        } catch {
            alert("Erro ao deletar pessoa.");
        }
    }

    return (
        <div className="bg-[#1f2937]/50 border border-gray-700/50 rounded-2xl p-6 shadow-xl min-h-[250px]">
            <h2 className="text-xl font-semibold mb-6">Pessoas Cadastradas</h2>
            <ul className="space-y-4">
                {pessoas.map((p) => (
                    <li key={p.id} className="flex justify-between items-center py-3 border-b border-gray-700/50">
                        <span className="text-gray-200">{p.nome} ({p.idade} anos)</span>
                        <button
                            className="text-red-400 hover:text-red-300 p-2 border border-gray-600 rounded-md transition-all"
                            onClick={() => handleDeletar(p.id)}
                        >
                            <svg className="h-5 w-5" fill="currentColor" viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg">
                                <path fillRule="evenodd" clipRule="evenodd" d="M9 2a1 1 0 00-.894.553L7.382 4H4a1 1 0 000 2v10a2 2 0 002 2h8a2 2 0 002-2V6a1 1 0 100-2h-3.382l-.724-1.447A1 1 0 0011 2H9zM7 8a1 1 0 012 0v6a1 1 0 11-2 0V8zm5-1a1 1 0 00-1 1v6a1 1 0 102 0V8a1 1 0 00-1-1z" />
                            </svg>
                        </button>
                    </li>
                ))}
            </ul>
        </div>
    );
}