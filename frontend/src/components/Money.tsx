interface Props {
    valor: number;
    tone?: "success" | "danger" | "neutral";
}

export function Money({ valor, tone = "neutral" }: Props) {
    const toneClass =
        tone === "success" ? "text-emerald-400" : tone === "danger" ? "text-red-400" : "text-gray-100";

    return (
        <span className={`font-mono-nums font-semibold ${toneClass}`}>
      {valor.toLocaleString("pt-BR", { style: "currency", currency: "BRL" })}
    </span>
    );
}