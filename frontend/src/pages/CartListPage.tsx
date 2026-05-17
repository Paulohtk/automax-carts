import { useEffect, useMemo, useState } from 'react'
import { CartFilters } from '../components/CartFilters'
import { CartTable } from '../components/CartTable'
import { getCarts, syncCarts } from '../services/api/cartsApi'
import type { Cart } from '../types/cart'

export function CartListPage() {
  const [carts, setCarts] = useState<Cart[]>([])
  const [isLoading, setIsLoading] = useState(true)
  const [isSyncing, setIsSyncing] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [syncMessage, setSyncMessage] = useState<string | null>(null)
  const [userIdFilter, setUserIdFilter] = useState('')
  const [startDateFilter, setStartDateFilter] = useState('')
  const [endDateFilter, setEndDateFilter] = useState('')

  useEffect(() => {
    let isMounted = true

    async function loadCarts() {
      try {
        const loadedCarts = await getCarts()
        if (isMounted) {
          setCarts(loadedCarts)
        }
      } catch (loadError) {
        if (isMounted) {
          setError(loadError instanceof Error ? loadError.message : 'Erro inesperado.')
        }
      } finally {
        if (isMounted) {
          setIsLoading(false)
        }
      }
    }

    loadCarts()

    return () => {
      isMounted = false
    }
  }, [])

  const filteredCarts = useMemo(() => {
    return carts.filter((cart) => {
      const cartDate = cart.date.slice(0, 10)
      const matchesUser = userIdFilter ? cart.userId === Number(userIdFilter) : true
      const matchesStartDate = startDateFilter ? cartDate >= startDateFilter : true
      const matchesEndDate = endDateFilter ? cartDate <= endDateFilter : true

      return matchesUser && matchesStartDate && matchesEndDate
    })
  }, [carts, endDateFilter, startDateFilter, userIdFilter])

  async function handleSync() {
    setIsSyncing(true)
    setError(null)
    setSyncMessage(null)

    try {
      const result = await syncCarts()
      const loadedCarts = await getCarts()
      setCarts(loadedCarts)
      setSyncMessage(formatCartCount(result.syncedCount, 'sincronizado'))
    } catch (syncError) {
      setError(syncError instanceof Error ? syncError.message : 'Erro inesperado.')
    } finally {
      setIsSyncing(false)
      setIsLoading(false)
    }
  }

  function clearFilters() {
    setUserIdFilter('')
    setStartDateFilter('')
    setEndDateFilter('')
  }

  return (
    <main className="page-shell">
      <header className="page-header">
        <div>
          <p className="eyebrow">Automax</p>
          <h1>Painel de carrinhos</h1>
        </div>
        <button className="primary-button" type="button" disabled={isSyncing} onClick={handleSync}>
          {isSyncing ? 'Sincronizando...' : 'Sincronizar dados'}
        </button>
      </header>

      <section className="content-section" aria-live="polite">
        <CartFilters
          userId={userIdFilter}
          startDate={startDateFilter}
          endDate={endDateFilter}
          onUserIdChange={setUserIdFilter}
          onStartDateChange={setStartDateFilter}
          onEndDateChange={setEndDateFilter}
          onClear={clearFilters}
        />

        <div className="list-summary">
          <span>{formatCartCount(filteredCarts.length, 'exibido')}</span>
          {syncMessage && <span>{syncMessage}</span>}
        </div>

        {isLoading && <p className="status-text">Carregando carrinhos...</p>}
        {!isLoading && error && <p className="error-text">{error}</p>}
        {!isLoading && !error && <CartTable carts={filteredCarts} />}
      </section>
    </main>
  )
}

function formatCartCount(count: number, status: string) {
  const noun = count === 1 ? 'carrinho' : 'carrinhos'
  const suffix = count === 1 ? status : `${status}s`

  return `${count} ${noun} ${suffix}.`
}
