const API_URL = "http://localhost:5062/api";

export async function apiGet<T>(path: string): Promise<T> {
    const response = await fetch(`${API_URL}${path}`);
    if (!response.ok) throw new Error(`Erro ao buscar ${path}`);
    return response.json();
}

export async function apiPost<T>(path: string, body: unknown): Promise<T> {
    const response = await fetch(`${API_URL}${path}`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(body),
    });

    if (!response.ok) {
        const errorText = await response.text();
        throw new Error(errorText || `Erro ao criar em ${path}`);
    }

    return response.json();
}

export async function buscarTransacoes(termo: string) {
    return apiGet<Transacao[]>(`/Transacoes/buscar?termo=${encodeURIComponent(termo)}`);
}

export async function ordenarTransacoes(criterio: string, ascendente: boolean) {
    return apiGet<Transacao[]>(`/Transacoes/ordenar?criterio=${criterio}&ascendente=${ascendente}`);
}

export async function apiDelete(path: string): Promise<void> {
    const response = await fetch(`${API_URL}${path}`, { method: "DELETE" });
    if (!response.ok) throw new Error(`Erro ao deletar ${path}`);
}