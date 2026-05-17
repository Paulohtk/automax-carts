import { cleanup, render, screen } from '@testing-library/react'
import { afterEach, describe, expect, it } from 'vitest'
import { CartTable } from './CartTable'

describe('CartTable', () => {
  afterEach(() => {
    cleanup()
  })

  it('renders carts with total product quantity', () => {
    render(
      <CartTable
        carts={[
          {
            id: 1,
            date: '2020-03-02T00:00:00.000Z',
            userId: 2,
            totalProducts: 4,
            products: [
              { productId: 10, quantity: 3 },
              { productId: 20, quantity: 1 },
            ],
          },
        ]}
      />,
    )

    expect(screen.getByText('ID do carrinho')).toBeInTheDocument()
    expect(screen.getByText('Data de criação')).toBeInTheDocument()
    expect(screen.getByText('1')).toBeInTheDocument()
    expect(screen.getByText('2')).toBeInTheDocument()
    expect(screen.getByText('4')).toBeInTheDocument()
  })

  it('renders empty state when there are no carts', () => {
    render(<CartTable carts={[]} />)

    expect(screen.getByText(/Nenhum carrinho armazenado/i)).toBeInTheDocument()
  })
})
