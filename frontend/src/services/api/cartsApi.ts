import type { Cart } from '../../types/cart'

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5007'

type SyncCartsResponse = {
  syncedCount: number
}

export async function getCarts(): Promise<Cart[]> {
  const response = await fetch(`${API_BASE_URL}/carts`)

  if (!response.ok) {
    throw new Error('Nao foi possivel carregar os carrinhos.')
  }

  return response.json()
}

export async function syncCarts(): Promise<SyncCartsResponse> {
  const response = await fetch(`${API_BASE_URL}/carts/sync`, {
    method: 'POST',
  })

  if (!response.ok) {
    throw new Error('Nao foi possivel sincronizar os carrinhos.')
  }

  return response.json()
}
