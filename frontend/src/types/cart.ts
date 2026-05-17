export type CartProduct = {
  productId: number
  quantity: number
}

export type Cart = {
  id: number
  date: string
  userId: number
  totalProducts: number
  products: CartProduct[]
}
