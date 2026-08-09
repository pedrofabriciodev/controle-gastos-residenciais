import { useEffect, useState } from "react";
import { apiGet } from "./services/api";
import type { Pessoa, Transacao, TotalGeral } from "./types";
import { Header } from "./components/Header";
import { StatCards } from "./components/StatCards";
import { PessoaForm } from "./components/PessoaForm";
import { PessoaList } from "./components/PessoaList";
import { TransacaoForm } from "./components/TransacaoForm";
import { TransacaoList } from "./components/TransacaoList";
import { ResumoFinanceiro } from "./components/ResumoFinanceiro";

function App() {
    const [pessoas, setPessoas] = useState<Pessoa[]>([]);
    const [transacoes, setTransacoes] = useState<Transacao[]>([]);
    const [totais, setTotais] = useState<TotalGeral | null>(null);

    async function carregarTudo() {
        const [pessoasData, transacoesData, totaisData] = await Promise.all([
            apiGet<Pessoa[]>("/Pessoas"),
            apiGet<Transacao[]>("/Transacoes"),
            apiGet<TotalGeral>("/Totais"),
        ]);
        setPessoas(pessoasData);
        setTransacoes(transacoesData);
        setTotais(totaisData);
    }

    useEffect(() => {
        carregarTudo();
    }, []);

    return (
        <div className="min-h-screen p-8" style={{ backgroundColor: "var(--bg)" }}>
            <Header saldoGeral={totais?.saldo ?? null} />
            {totais && <StatCards totais={totais} />}
            <main className="grid grid-cols-1 lg:grid-cols-3 gap-6 max-w-7xl mx-auto px-4">
                <section className="space-y-8">
                    <PessoaForm onPessoaCriada={() => carregarTudo()} />
                    <TransacaoForm pessoas={pessoas} onTransacaoCriada={() => carregarTudo()} />
                </section>

                <section className="space-y-8">
                    {totais && <ResumoFinanceiro totais={totais} />}
                    <PessoaList pessoas={pessoas} onPessoaRemovida={() => carregarTudo()} />
                </section>

                <section>
                    <TransacaoList transacoes={transacoes} pessoas={pessoas} />
                </section>
            </main>
        </div>
    );
}

export default App;