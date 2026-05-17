import { cleanup, render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, describe, expect, it, vi } from 'vitest'
import { CartListPage } from './CartListPage'

describe('CartListPage', () => {
  afterEach(() => {
    cleanup()
    vi.restoreAllMocks()
    vi.unstubAllGlobals()
  })

  it('loads and renders carts from the backend', async () => {
    vi.stubGlobal('fetch', vi.fn().mockResolvedValue(createResponse([createCart({ id: 3, userId: 5 })])))

    render(<CartListPage />)

    expect(screen.getByText('Carregando carrinhos...')).toBeInTheDocument()
    expect(await screen.findByText('3')).toBeInTheDocument()
    expect(screen.getByText('5')).toBeInTheDocument()
    expect(screen.getByText('7')).toBeInTheDocument()
  })

  it('filters carts by user and date range', async () => {
    const user = userEvent.setup()
    vi.stubGlobal(
      'fetch',
      vi.fn().mockResolvedValue(
        createResponse([
          createCart({ id: 1, userId: 5, date: '2020-03-02T00:00:00.000Z' }),
          createCart({ id: 2, userId: 6, date: '2020-03-02T00:00:00.000Z' }),
          createCart({ id: 3, userId: 5, date: '2020-04-10T00:00:00.000Z' }),
        ]),
      ),
    )

    render(<CartListPage />)

    expect(await screen.findByText('1')).toBeInTheDocument()
    await user.type(screen.getByLabelText('User ID'), '5')
    await user.type(screen.getByLabelText('Data inicial'), '2020-03-01')
    await user.type(screen.getByLabelText('Data final'), '2020-03-31')

    expect(screen.getByText('1 carrinho exibido.')).toBeInTheDocument()
    expect(screen.getByText('1')).toBeInTheDocument()
    expect(screen.queryByText('2')).not.toBeInTheDocument()
    expect(screen.queryByText('3')).not.toBeInTheDocument()
  })

  it('syncs carts from the page action and reloads the list', async () => {
    const user = userEvent.setup()
    const fetchMock = vi
      .fn()
      .mockResolvedValueOnce(createResponse([]))
      .mockResolvedValueOnce(createResponse({ syncedCount: 1 }))
      .mockResolvedValueOnce(createResponse([createCart({ id: 8, userId: 4 })]))
    vi.stubGlobal('fetch', fetchMock)

    render(<CartListPage />)

    expect(await screen.findByText(/Nenhum carrinho armazenado/i)).toBeInTheDocument()
    await user.click(screen.getByRole('button', { name: 'Sincronizar dados' }))

    expect(await screen.findByText('1 carrinho sincronizado.')).toBeInTheDocument()
    expect(screen.getByText('8')).toBeInTheDocument()
    expect(fetchMock).toHaveBeenCalledWith('http://localhost:5007/carts/sync', { method: 'POST' })
  })
})

function createCart({
  id,
  userId,
  date = '2020-03-02T00:00:00.000Z',
}: {
  id: number
  userId: number
  date?: string
}) {
  return {
    id,
    date,
    userId,
    totalProducts: 7,
    products: [{ productId: 1, quantity: 7 }],
  }
}

function createResponse(data: unknown) {
  return {
    ok: true,
    json: async () => data,
  }
}
