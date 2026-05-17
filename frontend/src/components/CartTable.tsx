import type { Cart } from '../types/cart'

type CartTableProps = {
  carts: Cart[]
}

export function CartTable({ carts }: CartTableProps) {
  if (carts.length === 0) {
    return (
      <p className="empty-state">
        Nenhum carrinho armazenado. Sincronize os dados no backend para popular o banco local.
      </p>
    )
  }

  return (
    <div className="table-wrapper">
      <table>
        <thead>
          <tr>
            <th>ID do carrinho</th>
            <th>Data de criação</th>
            <th>ID do usuário</th>
            <th>Quantidade total de produtos</th>
          </tr>
        </thead>
        <tbody>
          {carts.map((cart) => (
            <tr key={cart.id}>
              <td>{cart.id}</td>
              <td>{formatDate(cart.date)}</td>
              <td>{cart.userId}</td>
              <td>{cart.totalProducts}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}

function formatDate(value: string) {
  return new Intl.DateTimeFormat('pt-BR', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
  }).format(new Date(value))
}
